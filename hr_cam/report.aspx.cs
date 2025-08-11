using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace hr_cam
{
    public partial class report : System.Web.UI.Page
    {
        string ApiUrl = ConfigurationManager.AppSettings["ApiUrlReport"];
        string ApiUser = ConfigurationManager.AppSettings["ApiUser"];
        string ApiPassword = ConfigurationManager.AppSettings["ApiPassword"];
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["from_new_report"] != null)
                {
                    //Response.Write("<script>alert('masuk semua apa " + Session["from_new_report"].ToString() + "')</script>");
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                    Session["from_new_report"] = today;

                }
                if (Session["to_new_report"] != null)
                {
                    TextBox2.Text = Session["to_new_report"].ToString();
                }
                TextBox1.Text = Session["from_new_report"].ToString();
                //DropDownList1.SelectedValue = Session["site_report"].ToString();
                FillCard();
            }
        }

        protected void Button1_Clicked(object sender, EventArgs e)
        {
            string dari = TextBox1.Text;
            //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            DateTime fromDate;
            if (DateTime.TryParse(dari.ToString(), out fromDate))
            {
                //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                dari = today;

            }
            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            //Session["site_report"] = camera_site;
            Session["from_new_report"] = dari;
            Session["to_new_report"] = ke;
            Response.Redirect("report.aspx");
        }

        private void FillCard2()
        {
            string from = Session["from_new_report"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);

            int from_month = parsedDateTime.Month;
            int from_year = parsedDateTime.Year;
            DateTime firstDateOfMonth = new DateTime(from_year, from_month, 1);
            string bulan1 = firstDateOfMonth.ToString("yyyy-MM-dd 00:00:00");

            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            string bulan2 = null;
            if (Session["to_new_report"] != null && !string.IsNullOrEmpty(Session["to_new_report"].ToString()))
            {
                to = Session["to_new_report"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                int to_month = parsedDateTime2.Month;
                int to_year = parsedDateTime2.Year;
                int lastDay = DateTime.DaysInMonth(to_year, to_month);
                DateTime lastDateOfMonth = new DateTime(to_year, to_month, lastDay);
                bulan2 = lastDateOfMonth.ToString("yyyy-MM-dd 23:59:59");
            }
            else
            {

            }
            Response.Write(from_date);
            Response.Write(to_date);
            Response.Write(bulan1);
            Response.Write(bulan2);
        }
        private void FillCard()
        {
            string from = Session["from_new_report"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);

            int from_month = parsedDateTime.Month;
            int from_year = parsedDateTime.Year;
            DateTime firstDateOfMonth = new DateTime(from_year, from_month, 1);
            string bulan1 = firstDateOfMonth.ToString("yyyy-MM-dd 00:00:00");

            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            string bulan2 = null;
            if (Session["to_new_report"] != null && !string.IsNullOrEmpty(Session["to_new_report"].ToString()))
            {
                to = Session["to_new_report"].ToString();
                DateTime parsedDateTime2 = DateTime.Parse(to);
                to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                int to_month = parsedDateTime2.Month;
                int to_year = parsedDateTime2.Year;
                int lastDay = DateTime.DaysInMonth(to_year, to_month);
                DateTime lastDateOfMonth = new DateTime(to_year, to_month, lastDay);
                bulan2 = lastDateOfMonth.ToString("yyyy-MM-dd 23:59:59");
            }
            else
            {

            }
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string querycard1 = @"SELECT COUNT(*) AS jumlah FROM camera_events WHERE type like '%Face%'";
                if (from_date != null)
                {
                    querycard1 += @" AND occurred_at>=@from_date";
                }
                if (to_date != null)
                {
                    querycard1 += @" AND occurred_at<=@to_date";

                }
                using (MySqlCommand cmd = new MySqlCommand(querycard1, con))
                {
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
                                Label1.Text = dr.GetValue(0).ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                string querycard2 = @"SELECT COUNT(*) AS jumlah FROM camera_events c JOIN persons p ON c.person_identification_number=p.identification_number WHERE p.type=@type";
                if (from_date != null)
                {
                    querycard2 += @" AND occurred_at>=@from_date";
                }
                if (to_date != null)
                {
                    querycard2 += @" AND occurred_at<=@to_date";

                }
                using (MySqlCommand cmd2 = new MySqlCommand(querycard2, con))
                {
                    cmd2.Parameters.AddWithValue("@type", "blacklist");
                    if (from_date != null)
                    {
                        cmd2.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd2.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd2.Prepare();
                    using (MySqlDataReader dr2 = cmd2.ExecuteReader())
                    {
                        if (dr2.HasRows)
                        {
                            while (dr2.Read())
                            {
                                Label2.Text = dr2.GetValue(0).ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                string querycard3 = @"SELECT COUNT(*) as jumlah FROM camera_events WHERE type like '%Face%'";
                if (bulan1 != null)
                {
                    querycard3 += @" AND occurred_at >=@bulan1";
                }
                if (bulan2 != null)
                {
                    querycard3 += @" AND occurred_at <=@bulan2";

                }
                using (MySqlCommand cmd3 = new MySqlCommand(querycard3, con))
                {
                    if (bulan1 != null)
                    {
                        cmd3.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd3.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd3.Prepare();
                    using (MySqlDataReader dr3 = cmd3.ExecuteReader())
                    {
                        if (dr3.HasRows)
                        {
                            while (dr3.Read())
                            {
                                Label3.Text = dr3.GetValue(0).ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                string querycard4 = @"SELECT COUNT(*) AS jumlah FROM camera_events c JOIN persons p ON c.person_identification_number=p.identification_number WHERE p.type=@type";
                if (bulan1 != null)
                {
                    querycard4 += @" AND occurred_at >=@bulan1";
                }
                if (bulan2 != null)
                {
                    querycard4 += @" AND occurred_at <=@bulan2";

                }
                using (MySqlCommand cmd4 = new MySqlCommand(querycard4, con))
                {
                    cmd4.Parameters.AddWithValue("@type", "blacklist");
                    if (bulan1 != null)
                    {
                        cmd4.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd4.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd4.Prepare();
                    using (MySqlDataReader dr4 = cmd4.ExecuteReader())
                    {
                        if (dr4.HasRows)
                        {
                            while (dr4.Read())
                            {
                                Label4.Text = dr4.GetValue(0).ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                int ftw = 0;
                int ftw_month = 0;
                string query = @"SELECT ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code=0";
                string query2 = @"SELECT count(*) as jumlah, ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code!=0 AND ce.type like '%face%'";
                if (from_date != null)
                {
                    query += @" AND ce.occurred_at>=@from_date";
                    query2 += @" AND ce.occurred_at>=@from_date";
                }
                if (to_date != null)
                {
                    query += @" AND ce.occurred_at<=@to_date";
                    query2 += @" AND ce.occurred_at<=@to_date";
                }
                query += @" GROUP BY ce.person_identification_number";
                string query3 = @"SELECT ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code=0";
                string query4 = @"SELECT count(*) as jumlah, ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code!=0 AND ce.type like '%face%'";
                if (bulan1 != null)
                {
                    query3 += @" AND occurred_at>=@bulan1";
                    query4 += @" AND occurred_at>=@bulan1";
                }
                if (bulan2 != null)
                {
                    query3 += @" AND DATE(occurred_at) <= @bulan2";
                    query4 += @" AND DATE(occurred_at) <= @bulan2";
                }
                query3 += @" GROUP BY ce.person_identification_number";
                using (MySqlCommand cmd5 = new MySqlCommand(query, con))
                {
                    if (from_date != null)
                    {
                        cmd5.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd5.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd5.Prepare();
                    using (MySqlDataReader dr5 = cmd5.ExecuteReader())
                    {
                        if (dr5.HasRows)
                        {
                            while (dr5.Read())
                            {
                                ftw++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Label5.Text = ftw.ToString();

                using (MySqlCommand cmd6 = new MySqlCommand(query2, con))
                {
                    if (from_date != null)
                    {
                        cmd6.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd6.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd6.Prepare();
                    using (MySqlDataReader dr6 = cmd6.ExecuteReader())
                    {
                        if (dr6.HasRows)
                        {
                            while (dr6.Read())
                            {
                                Label6.Text = dr6["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd7 = new MySqlCommand(query3, con))
                {
                    if (bulan1 != null)
                    {
                        cmd7.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd7.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd7.Prepare();
                    using (MySqlDataReader dr7 = cmd7.ExecuteReader())
                    {
                        if (dr7.HasRows)
                        {
                            while (dr7.Read())
                            {
                                ftw_month++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Label7.Text = ftw_month.ToString();

                using (MySqlCommand cmd8 = new MySqlCommand(query4, con))
                {
                    if (bulan1 != null)
                    {
                        cmd8.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd8.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd8.Prepare();
                    using (MySqlDataReader dr8 = cmd8.ExecuteReader())
                    {
                        if (dr8.HasRows)
                        {
                            while (dr8.Read())
                            {
                                Label8.Text = dr8["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                string query5 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=0";
                string query6 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=1";
                string query7 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=2";
                string query8 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and plate_number not in (SELECT plate_number from vehicles) and (plate_number!='' or plate_number!=null)";
                string query9 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and (plate_number='' or plate_number=null or plate_number IS NULL)";
                string query15 = @"SELECT count(*) as jumlah from camera_events where type like 'Intrusion%'";
                if (from_date != null)
                {
                    query5 += @" and ce.occurred_at>=@from_date";
                    query6 += @" and ce.occurred_at>=@from_date";
                    query7 += @" and ce.occurred_at>=@from_date";
                    query8 += @" and occurred_at>=@from_date";
                    query9 += @" and occurred_at>=@from_date";
                    query15 += @" and occurred_at>=@from_date";
                }
                if (to_date != null)
                {
                    query5 += @" and ce.occurred_at<=@to_date";
                    query6 += @" and ce.occurred_at<=@to_date";
                    query7 += @" and ce.occurred_at<=@to_date";
                    query8 += @" and occurred_at<=@to_date";
                    query9 += @" and occurred_at<=@to_date";
                    query15 += @" and occurred_at<=@to_date";
                }
                query5 += @" GROUP BY ce.plate_number";
                query6 += @" GROUP BY ce.plate_number";
                query7 += @" GROUP BY ce.plate_number";
                query8 += @" group by plate_number";

                string query10 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=0";
                string query11 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=1";
                string query12 = @"SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and vh.entry_code=2";
                string query13 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and plate_number not in (SELECT plate_number from vehicles) and (plate_number!='' or plate_number!=null)";
                string query14 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and (plate_number='' or plate_number=null or plate_number IS NULL)";
                string query16 = @"SELECT count(*) as jumlah from camera_events where type like 'Intrusion%'";
                if (bulan1 != null)
                {
                    query10 += @" and ce.occurred_at>=@bulan1";
                    query11 += @" and ce.occurred_at>=@bulan1";
                    query12 += @" and ce.occurred_at>=@bulan1";
                    query13 += @" and occurred_at>=@bulan1";
                    query14 += @" and occurred_at>=@bulan1";
                    query16 += @" and occurred_at>=@bulan1";
                }
                if (bulan2 != null)
                {
                    query10 += @" and ce.occurred_at<=@bulan2";
                    query11 += @" and ce.occurred_at<=@bulan2";
                    query12 += @" and ce.occurred_at<=@bulan2";
                    query13 += @" and occurred_at<=@bulan2";
                    query14 += @" and occurred_at<=@bulan2";
                    query16 += @" and occurred_at<=@bulan2";
                }
                query10 += @" GROUP BY ce.plate_number";
                query11 += @" GROUP BY ce.plate_number";
                query12 += @" GROUP BY ce.plate_number";
                query13 += @" group by plate_number";
                int tambah1 = 0;
                int tambah2 = 0;
                int tambah3 = 0;
                int tambah4 = 0;
                int tambah5 = 0;
                int tambah6 = 0;
                int tambah7 = 0;
                int tambah8 = 0;
                int tambah9 = 0;
                int tambah10 = 0;
                using (MySqlCommand cmd13 = new MySqlCommand(query5, con))
                {
                    if (from_date != null)
                    {
                        cmd13.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd13.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd13.Prepare();
                    using (MySqlDataReader dr13 = cmd13.ExecuteReader())
                    {
                        if (dr13.HasRows)
                        {
                            while (dr13.Read())
                            {
                                tambah1++;
                            }
                        }
                        else
                        {
                        }
                    }

                }
                Label13.Text = tambah1.ToString();
                using (MySqlCommand cmd14 = new MySqlCommand(query6, con))
                {
                    if (from_date != null)
                    {
                        cmd14.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd14.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd14.Prepare();
                    using (MySqlDataReader dr14 = cmd14.ExecuteReader())
                    {
                        if (dr14.HasRows)
                        {
                            while (dr14.Read())
                            {
                                tambah2++;
                            }
                        }
                        else
                        {
                        }
                    }

                }
                Label14.Text = tambah2.ToString();
                using (MySqlCommand cmd14 = new MySqlCommand(query7, con))
                {
                    if (from_date != null)
                    {
                        cmd14.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd14.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd14.Prepare();
                    using (MySqlDataReader dr14 = cmd14.ExecuteReader())
                    {
                        if (dr14.HasRows)
                        {
                            while (dr14.Read())
                            {
                                tambah3++;
                            }
                        }
                        else
                        {
                        }
                    }

                }
                using (MySqlCommand cmd10 = new MySqlCommand(query8, con))
                {
                    if (from_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd10.Prepare();
                    using (MySqlDataReader dr10 = cmd10.ExecuteReader())
                    {
                        if (dr10.HasRows)
                        {
                            while (dr10.Read())
                            {
                                tambah4++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd10 = new MySqlCommand(query9, con))
                {
                    if (from_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd10.Prepare();
                    using (MySqlDataReader dr10 = cmd10.ExecuteReader())
                    {
                        if (dr10.HasRows)
                        {
                            while (dr10.Read())
                            {
                                tambah5 += Convert.ToInt32(dr10["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                }
                int label10 = tambah4 + tambah5;
                int label9 = tambah1 + tambah2 + tambah3 + label10;
                Label10.Text = label10.ToString();
                Label9.Text = label9.ToString();

                using (MySqlCommand cmd11 = new MySqlCommand(query10, con))
                {
                    if (bulan1 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                tambah6++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Label16.Text = tambah6.ToString();
                using (MySqlCommand cmd11 = new MySqlCommand(query11, con))
                {
                    if (bulan1 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                tambah7++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Label17.Text = tambah7.ToString();
                using (MySqlCommand cmd11 = new MySqlCommand(query12, con))
                {
                    if (bulan1 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                tambah8++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd12 = new MySqlCommand(query13, con))
                {
                    if (bulan1 != null)
                    {
                        cmd12.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd12.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd12.Prepare();
                    using (MySqlDataReader dr12 = cmd12.ExecuteReader())
                    {
                        if (dr12.HasRows)
                        {
                            while (dr12.Read())
                            {
                                tambah9++;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd11 = new MySqlCommand(query14, con))
                {
                    if (bulan1 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                tambah10 += Convert.ToInt32(dr11["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                }
                int label12 = tambah9 + tambah10;
                Label12.Text = label12.ToString();
                int label11 = tambah6 + tambah7 + tambah8 + label12;
                Label11.Text = label11.ToString();
                using (MySqlCommand cmd10 = new MySqlCommand(query15, con))
                {
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    if (from_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@from_date", from_date);
                    }
                    if (to_date != null)
                    {
                        cmd10.Parameters.AddWithValue("@to_date", to_date);
                    }
                    cmd10.Prepare();
                    using (MySqlDataReader dr10 = cmd10.ExecuteReader())
                    {
                        if (dr10.HasRows)
                        {
                            while (dr10.Read())
                            {
                                Label19.Text = dr10["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd11 = new MySqlCommand(query16, con))
                {
                    if (bulan1 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan1", bulan1);
                    }
                    if (bulan2 != null)
                    {
                        cmd11.Parameters.AddWithValue("@bulan2", bulan2);
                    }
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                Label20.Text = dr11["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
    }
}