using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;


namespace hr_cam
{
    public partial class Home : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        protected void Page_Load(object sender, EventArgs e)
        {

            //Response.Write("<script>alert('" + UrlImage + "')</script>");
            TextBox1.TextMode = TextBoxMode.Date;
            TextBox2.TextMode = TextBoxMode.Date;
            if (!IsPostBack)
            {
                if (Session["from"] != null)
                {
                    //string from = Session["from"].ToString();
                    //DateTime fromDate;
                    //if (DateTime.TryParse(from.ToString(), out fromDate))
                    //{
                    //    //Response.Write("From" + Session["from"]);
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    Session["from"] = today;
                    //Response.Write("<script>alert('" + Session["from"].ToString() + "')</script>");

                }
                //}
                if (Session["to"] != null)
                {
                    //Response.Write("To" + Session["to"]);
                    TextBox2.Text = Session["to"].ToString();
                }
                TextBox1.Text = Session["from"].ToString();
                int limit = 20; // Default limit
                if (Session["queryLimit"] != null)
                {
                    limit = Convert.ToInt32(Session["queryLimit"]);
                }
                TextBox3.Text = limit.ToString();
                FillCard();
                FillEventHistoryDPO();
                FillEventHistoryTraffic();
                FillEventHistory();
                //FillChart1();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string dari = TextBox1.Text;
            //Response.Write("<script>alert('button " + dari + "')</script>");
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
            if (DateTime.TryParse(dari.ToString(), out _))
            {
                // Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            }
            else
            {
                dari = DateTime.Now.ToString("yyyy-MM-dd");
            }
            int limit = 20; // Default value
            if (!string.IsNullOrEmpty(TextBox3.Text) && int.TryParse(TextBox3.Text, out int parsedLimit))
            {
                limit = parsedLimit;
            }

            // Simpan nilai limit di Session
            Session["queryLimit"] = limit;

            //Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            string ke = TextBox2.Text;
            Session["from"] = dari;
            Session["to"] = ke;
            Response.Redirect("home.aspx");
        }

        private void FillCard()
        {
            DateTime fromDate = DateTime.Now;
            DateTime yesterday = fromDate.AddDays(-1);
            DateTime lastWeek = fromDate.AddDays(-7);
            DateTime last30Days = fromDate.AddDays(-30);
            string from = fromDate.ToString("yyyy-MM-dd");
            string first_date = from.Substring(0, 4) + "-" + from.Substring(5, 2) + "-" + "01";
            string kemarin = yesterday.ToString("yyyy-MM-dd");
            string seminggu = lastWeek.ToString("yyyy-MM-dd");
            string sebulan = last30Days.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events WHERE type like '%Face%' AND DATE(occurred_at)=@tanggal;", con))
                {
                    cmd.Parameters.AddWithValue("@tanggal", from);
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
                using (MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events c JOIN persons p ON c.person_identification_number=p.identification_number WHERE p.type=@type AND DATE(occurred_at)=@tanggal", con))
                {
                    cmd2.Parameters.AddWithValue("@type", "blacklist");
                    cmd2.Parameters.AddWithValue("@tanggal", from);
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
                using (MySqlCommand cmd3 = new MySqlCommand("SELECT COUNT(*) as jumlah FROM camera_events WHERE type like '%Face%' AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from", con))
                {
                    cmd3.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd3.Parameters.AddWithValue("@from", from);
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
                using (MySqlCommand cmd4 = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events c JOIN persons p ON c.person_identification_number=p.identification_number WHERE p.type=@type AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from", con))
                {
                    cmd4.Parameters.AddWithValue("@type", "blacklist");
                    cmd4.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd4.Parameters.AddWithValue("@from", from);
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
                string query = @"SELECT count(*) as jumlah from (SELECT ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code=0 AND DATE(occurred_at)=@tanggal GROUP BY ce.person_identification_number) as jumlah";
                string query2 = @"SELECT count(*) as jumlah, ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code!=0 AND DATE(ce.occurred_at)=@tanggal and ce.type like '%face%'";
                string query3 = @"SELECT count(*) as jumlah from (SELECT ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code=0 AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from GROUP BY ce.person_identification_number) as jumlah";
                string query4 = @"SELECT count(*) as jumlah, ce.type, ce.camera_id, p.identification_number, ph.entry_code  FROM camera_events as ce JOIN persons p ON ce.person_identification_number=p.identification_number JOIN person_history ph ON p.id = ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) where (ce.image_file != '' OR ce.image_file != NULL) AND ph.entry_code!=0 AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from and ce.type like '%face%'";
                using (MySqlCommand cmd5 = new MySqlCommand(query, con))
                {
                    //cmd5.Parameters.AddWithValue("@type", "Face Recognition Match");
                    cmd5.Parameters.AddWithValue("@tanggal", from);
                    cmd5.Prepare();
                    using (MySqlDataReader dr5 = cmd5.ExecuteReader())
                    {
                        if (dr5.HasRows)
                        {
                            while (dr5.Read())
                            {
                                //ftw++;
                                Label5.Text = dr5["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd6 = new MySqlCommand(query2, con))
                {
                    //cmd6.Parameters.AddWithValue("@type", "Face Recognition Not Match");
                    cmd6.Parameters.AddWithValue("@tanggal", from);
                    cmd6.Prepare();
                    using (MySqlDataReader dr6 = cmd6.ExecuteReader())
                    {
                        if (dr6.HasRows)
                        {
                            while (dr6.Read())
                            {
                                //nftw++;
                                Label6.Text = dr6["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                //Label6.Text = nftw.ToString();

                using (MySqlCommand cmd7 = new MySqlCommand(query3, con))
                {
                    //cmd7.Parameters.AddWithValue("@type", "Face Recognition Match");
                    cmd7.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd7.Parameters.AddWithValue("@from", from);
                    cmd7.Prepare();
                    using (MySqlDataReader dr7 = cmd7.ExecuteReader())
                    {
                        if (dr7.HasRows)
                        {
                            while (dr7.Read())
                            {
                                //ftw_month++;
                                Label7.Text = dr7["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                using (MySqlCommand cmd8 = new MySqlCommand(query4, con))
                {
                    //cmd8.Parameters.AddWithValue("@type", "Face Recognition Not Match");
                    cmd8.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd8.Parameters.AddWithValue("@from", from);
                    cmd8.Prepare();
                    using (MySqlDataReader dr8 = cmd8.ExecuteReader())
                    {
                        if (dr8.HasRows)
                        {
                            while (dr8.Read())
                            {
                                //nftw_month++;
                                Label8.Text = dr8["jumlah"].ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                //Label8.Text = nftw_month.ToString();
                //int lpr = 0;
                //int lpur = 0;
                //int lpr_month = 0;
                //int lpur_month = 0;
                //using (MySqlCommand cmd9 = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events WHERE type like '%plate%' AND  plate_number in (SELECT plate_number from vehicles) AND DATE(occurred_at)=@tanggal GROUP BY plate_number", con))
                //{
                //    //cmd9.Parameters.AddWithValue("@type", "Plate Match");
                //    cmd9.Parameters.AddWithValue("@tanggal", from);
                //    cmd9.Prepare();
                //    using (MySqlDataReader dr9 = cmd9.ExecuteReader())
                //    {
                //        if (dr9.HasRows)
                //        {
                //            while (dr9.Read())
                //            {
                //                lpr++;
                //            }
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                //Label9.Text = lpr.ToString();

                string query5 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at)=@tanggal and vh.entry_code=0 GROUP BY ce.plate_number) as jumlah";
                string query6 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at)=@tanggal and vh.entry_code=1 GROUP BY ce.plate_number) as jumlah";
                string query7 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at)=@tanggal and vh.entry_code=2 GROUP BY ce.plate_number) as jumlah";
                string query8 = @"SELECT count(*) as jumlah from (SELECT count(*) as jumlah2 from camera_events where type like '%plate%' and plate_number not in (SELECT plate_number from vehicles) and (plate_number!='' or plate_number!=null) and DATE(occurred_at)=@tanggal group by plate_number) as jumlah";
                string query9 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and (plate_number='' or plate_number=null or plate_number IS NULL) and DATE(occurred_at)=@tanggal";

                string query10 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at) >=@tgl_1 AND DATE(ce.occurred_at) <= @from and vh.entry_code=0 GROUP BY ce.plate_number) as jumlah";
                string query11 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at) >=@tgl_1 AND DATE(ce.occurred_at) <= @from and vh.entry_code=1 GROUP BY ce.plate_number) as jumlah";
                string query12 = @"SELECT count(*) as jumlah from (SELECT count(DISTINCT(ce.plate_number)), ce.type, ce.plate_number, v.id, vh.entry_code FROM camera_events ce JOIN vehicles v on ce.plate_number=v.plate_number JOIN vehicle_history vh ON v.id=vh.vehicle_id AND vh.created_at = (SELECT MAX(vh2.created_at) FROM vehicle_history vh2 WHERE vh2.vehicle_id = v.id AND vh2.created_at <= ce.occurred_at) where ce.type like '%plate%' and DATE(ce.occurred_at) >=@tgl_1 AND DATE(ce.occurred_at) <= @from and vh.entry_code=2 GROUP BY ce.plate_number) as jumlah";
                string query13 = @"SELECT count(*) as jumlah from (SELECT count(*) as jumlah2 from camera_events where type like '%plate%' and plate_number not in (SELECT plate_number from vehicles) and (plate_number!='' or plate_number!=null) and DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from group by plate_number) as jumlah";
                string query14 = @"SELECT count(*) as jumlah from camera_events where type like '%plate%' and (plate_number='' or plate_number=null or plate_number IS NULL) and DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from";

                string query15 = @"SELECT count(*) as jumlah from camera_events where DATE(occurred_at)=@tanggal and type like 'Intrusion%'";
                string query16 = @"SELECT count(*) as jumlah from camera_events where DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from and type like 'Intrusion%'";
                //using (MySqlCommand cmd10 = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events WHERE  type=@type AND DATE(occurred_at)=@tanggal GROUP BY plate_number;", con))
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
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    cmd13.Parameters.AddWithValue("@tanggal", from);
                    cmd13.Prepare();
                    using (MySqlDataReader dr13 = cmd13.ExecuteReader())
                    {
                        if (dr13.HasRows)
                        {
                            while (dr13.Read())
                            {
                                //tambah1++;
                                tambah1 = Convert.ToInt32(dr13["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                    //int totalRecords = Convert.ToInt32(cmd13.ExecuteScalar());

                }
                Label13.Text = tambah1.ToString();
                using (MySqlCommand cmd14 = new MySqlCommand(query6, con))
                {
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    cmd14.Parameters.AddWithValue("@tanggal", from);
                    cmd14.Prepare();
                    using (MySqlDataReader dr14 = cmd14.ExecuteReader())
                    {
                        if (dr14.HasRows)
                        {
                            while (dr14.Read())
                            {
                                //tambah2++;
                                tambah2 = Convert.ToInt32(dr14["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                    //int totalRecords = Convert.ToInt32(cmd13.ExecuteScalar());

                }
                Label14.Text = tambah2.ToString();
                using (MySqlCommand cmd14 = new MySqlCommand(query7, con))
                {
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    cmd14.Parameters.AddWithValue("@tanggal", from);
                    cmd14.Prepare();
                    using (MySqlDataReader dr14 = cmd14.ExecuteReader())
                    {
                        if (dr14.HasRows)
                        {
                            while (dr14.Read())
                            {
                                //tambah3++;
                                tambah3 = Convert.ToInt32(dr14["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                    //int totalRecords = Convert.ToInt32(cmd13.ExecuteScalar());

                }
                //Label15.Text = tambah3.ToString();
                using (MySqlCommand cmd10 = new MySqlCommand(query8, con))
                {
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    cmd10.Parameters.AddWithValue("@tanggal", from);
                    cmd10.Prepare();
                    using (MySqlDataReader dr10 = cmd10.ExecuteReader())
                    {
                        if (dr10.HasRows)
                        {
                            while (dr10.Read())
                            {
                                //tambah4++;
                                tambah4 = Convert.ToInt32(dr10["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd10 = new MySqlCommand(query9, con))
                {
                    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                    cmd10.Parameters.AddWithValue("@tanggal", from);
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
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd11.Parameters.AddWithValue("@from", from);
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                //tambah6++;
                                tambah6 = Convert.ToInt32(dr11["jumlah"].ToString());
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
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd11.Parameters.AddWithValue("@from", from);
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                //tambah7++;
                                tambah7 = Convert.ToInt32(dr11["jumlah"].ToString());
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
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd11.Parameters.AddWithValue("@from", from);
                    cmd11.Prepare();
                    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                    {
                        if (dr11.HasRows)
                        {
                            while (dr11.Read())
                            {
                                //tambah8++;
                                tambah8 = Convert.ToInt32(dr11["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                }
                //Label18.Text = tambah8.ToString();
                using (MySqlCommand cmd12 = new MySqlCommand(query13, con))
                {
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd12.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd12.Parameters.AddWithValue("@from", from);
                    cmd12.Prepare();
                    using (MySqlDataReader dr12 = cmd12.ExecuteReader())
                    {
                        if (dr12.HasRows)
                        {
                            while (dr12.Read())
                            {
                                //tambah9++;
                                tambah9 = Convert.ToInt32(dr12["jumlah"].ToString());
                            }
                        }
                        else
                        {
                        }
                    }
                }
                using (MySqlCommand cmd11 = new MySqlCommand(query14, con))
                {
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd11.Parameters.AddWithValue("@from", from);
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
                    cmd10.Parameters.AddWithValue("@tanggal", from);
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
                    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                    cmd11.Parameters.AddWithValue("@from", from);
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

                //using (MySqlCommand cmd10 = new MySqlCommand("SELECT COUNT(*) AS jumlah FROM camera_events WHERE type like '%plate%' AND (plate_number not in (SELECT plate_number from vehicles) or plate_number='' or plate_number=NULL or plate_number IS NULL) AND DATE(occurred_at)=@tanggal GROUP BY plate_number;", con))
                //{
                //    //cmd10.Parameters.AddWithValue("@type", "Plate MisMatch");
                //    cmd10.Parameters.AddWithValue("@tanggal", from);
                //    cmd10.Prepare();
                //    using (MySqlDataReader dr10 = cmd10.ExecuteReader())
                //    {
                //        if (dr10.HasRows)
                //        {
                //            while (dr10.Read())
                //            {
                //                lpur++;
                //            }
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                //Label10.Text = lpur.ToString();

                //using (MySqlCommand cmd11 = new MySqlCommand("SELECT COUNT(*) as jumlah FROM camera_events WHERE type like '%plate%' AND plate_number in (SELECT plate_number from vehicles) AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from GROUP BY plate_number", con))
                //{
                //    //cmd11.Parameters.AddWithValue("@type", "Plate Match");
                //    cmd11.Parameters.AddWithValue("@tgl_1", first_date);
                //    cmd11.Parameters.AddWithValue("@from", from);
                //    cmd11.Prepare();
                //    using (MySqlDataReader dr11 = cmd11.ExecuteReader())
                //    {
                //        if (dr11.HasRows)
                //        {
                //            while (dr11.Read())
                //            {
                //                lpr_month++;
                //            }
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                //Label11.Text = lpr_month.ToString();

                //using (MySqlCommand cmd12 = new MySqlCommand("SELECT COUNT(*) as jumlah FROM camera_events WHERE type like '%plate%' AND (plate_number not in (SELECT plate_number from vehicles) or plate_number='' or plate_number=NULL or plate_number IS NULL) AND DATE(occurred_at) >=@tgl_1 AND DATE(occurred_at) <= @from GROUP BY plate_number", con))
                //{
                //    //cmd12.Parameters.AddWithValue("@type", "Plate MisMatch");
                //    cmd12.Parameters.AddWithValue("@tgl_1", first_date);
                //    cmd12.Parameters.AddWithValue("@from", from);
                //    cmd12.Prepare();
                //    using (MySqlDataReader dr12 = cmd12.ExecuteReader())
                //    {
                //        if (dr12.HasRows)
                //        {
                //            while (dr12.Read())
                //            {
                //                lpur_month++;
                //            }
                //        }
                //        else
                //        {
                //        }
                //    }
                //}
                //Label12.Text = lpur_month.ToString();
            }
        }

        //private void FillEventHistoryDPO()
        //{
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
        //        if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to and p.type='blacklist' ORDER BY ce.occurred_at ASC", con))
        //            {
        //                cmd.Parameters.AddWithValue("@from", from);
        //                cmd.Parameters.AddWithValue("@to", to);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        //string imagePath = UrlImage + image_file;
        //                                        //string imagePath = UrlImage + image_file;
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            // ImageUrl = "data:image/png;base64," + base64String,
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };

        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };


        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //        else if (DateTime.TryParse(from.ToString(), out fromDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from and p.type='blacklist' ORDER BY ce.occurred_at ASC", con))
        //            {
        //                cmd.Parameters.AddWithValue("@from", from);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };



        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);
        //                                //row.Cells.Add(infoCell);

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
        //        else if (DateTime.TryParse(to, out toDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to and p.type='blacklist' ORDER BY ce.occurred_at ASC", con))
        //            {
        //                cmd.Parameters.AddWithValue("@to", to);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };


        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);

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

        //private void FillEventHistory()
        //{
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
        //        if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND DATE(ce.occurred_at) <=@to AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ORDER BY ce.occurred_at DESC limit 0,20", con))
        //            {
        //                cmd.Parameters.AddWithValue("@from", from);
        //                cmd.Parameters.AddWithValue("@to", to);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };

        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                    }
        //                                    imageCell.ColumnSpan = 2;
        //                                    row.Cells.Add(imageCell);
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };


        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                //row.Cells.Add(imageCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //        else if (DateTime.TryParse(from.ToString(), out fromDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) >=@from AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ORDER BY ce.occurred_at DESC limit 0,20", con))
        //            {
        //                cmd.Parameters.AddWithValue("@from", from);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };



        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);
        //                                //row.Cells.Add(infoCell);

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
        //        else if (DateTime.TryParse(to, out toDate))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND DATE(ce.occurred_at) <=@to AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ORDER BY ce.occurred_at DESC limit 0,20", con))
        //            {
        //                cmd.Parameters.AddWithValue("@to", to);
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
        //                            if (person_type == "blacklist")
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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);



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
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        // string imagePath = UrlImage + image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        // string imagePath2 = UrlImage + plate_number_file;
        //                                        // byte[] imageBytes2 = File.ReadAllBytes(imagePath2);
        //                                        // string base64String2 = Convert.ToBase64String(imageBytes2);
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
        //                                        // string imagePath = UrlImage+image_file;
        //                                        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        //                                        // string base64String = Convert.ToBase64String(imageBytes);
        //                                        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //                                        //img.ImageUrl = "data:image/png;base64," + base64String;
        //                                        //img.AlternateText = "icon title";
        //                                        //img.Style.Add("width", "150px");
        //                                        //imageCell.Controls.Add(img);
        //                                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
        //                                        {
        //                                            //ImageUrl = "data:image/png;base64," + base64String,
        //                                            ImageUrl = "image_file/" + image_file,
        //                                            AlternateText = "icon title"
        //                                        };
        //                                        img.Style.Add("width", "150px");
        //                                        imageCell.Controls.Add(img);
        //                                        imageCell.ColumnSpan = 2;
        //                                        row.Cells.Add(imageCell);
        //                                    }
        //                                    pers.Text = person;
        //                                }

        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //TableCell occurred = new TableCell();
        //                                //occurred.Text = occurred_at;
        //                                //TableCell loc = new TableCell();
        //                                //loc.Text = location;
        //                                //TableCell sites = new TableCell();
        //                                //sites.Text = site;
        //                                TableCell occurred = new TableCell() { Text = occurred_at };
        //                                TableCell loc = new TableCell() { Text = location };
        //                                TableCell sites = new TableCell() { Text = site };
        //                                TableCell status = new TableCell() { Text = event_type };


        //                                //TableCell infoCell = new TableCell();
        //                                //infoCell.Text = "<span style='font-size: 9px;'>" + occurred_at + "</span> <br>" +
        //                                //            "<span style='font-size: 15px;'>" + person + "</span> <br>" +
        //                                //            "<span style='font-size: 10px;'>Zona : " + location + "</span>";
        //                                //row.Cells.Add(infoCell);
        //                                row.Cells.Add(occurred);
        //                                row.Cells.Add(loc);
        //                                row.Cells.Add(sites);
        //                                row.Cells.Add(pers);
        //                                row.Cells.Add(status);

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
        private void FillEventHistoryTraffic()
        {
            string from = Session["from"].ToString();
            string to = null;
            if (Session["to"] != null)
            {
                to = Session["to"].ToString();
            }
            int limit = 20; // Default limit
            if (Session["queryLimit"] != null)
            {
                limit = Convert.ToInt32(Session["queryLimit"]);
            }

            DateTime fromDate;
            DateTime toDate;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
                {
                }
                else if (DateTime.TryParse(from.ToString(), out fromDate)) { }
                else if (DateTime.TryParse(to, out toDate)) { }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type like 'Intrusion%'";
                if (DateTime.TryParse(from.ToString(), out fromDate))
                {
                    query += @" AND DATE(ce.occurred_at) >=@from";
                }
                if (DateTime.TryParse(to, out toDate))
                {
                    query += @" AND DATE(ce.occurred_at) <=@to";
                }
                query += $" ORDER BY ce.occurred_at ASC limit {limit}";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (DateTime.TryParse(from.ToString(), out fromDate))
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                    }
                    if (DateTime.TryParse(to, out toDate))
                    {
                        cmd.Parameters.AddWithValue("@to", to);
                    }
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                string image_file = dr["image_file"].ToString();
                                DateTime occurredAt = Convert.ToDateTime(dr["occurred_at"]);
                                string occurred_at = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string location = dr["location"].ToString();
                                string site = dr["site"].ToString();

                                TableRow row = new TableRow();
                                TableCell imageCell = new TableCell();
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
                                TableCell occurred = new TableCell() { Text = occurred_at };
                                TableCell loc = new TableCell() { Text = location };
                                TableCell sites = new TableCell() { Text = site };

                                row.Cells.Add(imageCell);
                                row.Cells.Add(occurred);
                                row.Cells.Add(loc);
                                row.Cells.Add(sites);



                                Tbody2.Controls.Add(row);


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
        private void FillEventHistoryDPO()
        {
            string from = Session["from"].ToString();
            string to = null;
            if (Session["to"] != null)
            {
                to = Session["to"].ToString();
            }
            int limit = 20; // Default limit
            if (Session["queryLimit"] != null)
            {
                limit = Convert.ToInt32(Session["queryLimit"]);
            }

            DateTime fromDate;
            DateTime toDate;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
                {
                }
                else if (DateTime.TryParse(from.ToString(), out fromDate)) { }
                else if (DateTime.TryParse(to, out toDate)) { }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type,p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id WHERE (ce.image_file !='' or ce.image_file != NULL) AND p.type='blacklist'";
                if (DateTime.TryParse(from.ToString(), out fromDate))
                {
                    query += @" AND DATE(ce.occurred_at) >=@from";
                }
                if (DateTime.TryParse(to, out toDate))
                {
                    query += @" AND DATE(ce.occurred_at) <=@to";
                }
                query += $" ORDER BY ce.occurred_at ASC limit {limit}";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (DateTime.TryParse(from.ToString(), out fromDate))
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                    }
                    if (DateTime.TryParse(to, out toDate))
                    {
                        cmd.Parameters.AddWithValue("@to", to);
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
                                if (person_type == "blacklist")
                                {
                                    TableRow row = new TableRow();

                                    TableCell imageCell = new TableCell();
                                    TableCell imageCell2 = new TableCell();
                                    TableCell pers = new TableCell();
                                    int cek = event_type.IndexOf("Plate");

                                    if (cek != -1)
                                    {
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
                                    }

                                    TableCell occurred = new TableCell() { Text = occurred_at };
                                    TableCell loc = new TableCell() { Text = location };
                                    TableCell sites = new TableCell() { Text = site };

                                    row.Cells.Add(occurred);
                                    row.Cells.Add(loc);
                                    row.Cells.Add(sites);
                                    row.Cells.Add(pers);
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Ambil nilai dari TextBox3
            int limit = 20; // Default value
            if (!string.IsNullOrEmpty(TextBox3.Text) && int.TryParse(TextBox3.Text, out int parsedLimit))
            {
                limit = parsedLimit;
            }

            // Simpan nilai limit di Session
            Session["queryLimit"] = limit;

            // Panggil fungsi FillEventHistory untuk mengambil data dengan limit yang baru
            FillEventHistory();
        }
        private void FillEventHistory()
        {
            string from = Session["from"].ToString();
            string to = null;
            if (Session["to"] != null)
            {
                to = Session["to"].ToString();
            }

            int limit = 20; // Default limit
            if (Session["queryLimit"] != null)
            {
                limit = Convert.ToInt32(Session["queryLimit"]);
            }

            DateTime fromDate;
            DateTime toDate;

            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = @"SELECT ce.camera_id, ce.type AS event_type,ce.occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name LEFT JOIN persons as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at) WHERE (ce.image_file !='' or ce.image_file != NULL) AND ce.type='Face Recognition Match' AND (ph.entry_code='1' or ph.entry_code='2')";
                if (DateTime.TryParse(from.ToString(), out fromDate))
                {
                    query += @" AND DATE(ce.occurred_at) >=@from";
                }
                if (DateTime.TryParse(to, out toDate))
                {
                    query += @" AND DATE(ce.occurred_at) <=@to";
                }
                query += @" AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ";
                query += " ORDER BY ce.occurred_at DESC ";
                query += $" LIMIT {limit}";
                //Response.Write(query);
                //query += @" AND (ce.person_identification_number IS NULL OR ce.person_identification_number = '' OR p.type IS NULL OR p.type != 'blacklist') ORDER BY ce.occurred_at DESC limit 0,20";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    if (DateTime.TryParse(from.ToString(), out fromDate))
                    {
                        cmd.Parameters.AddWithValue("@from", from);
                    }
                    if (DateTime.TryParse(to, out toDate))
                    {
                        cmd.Parameters.AddWithValue("@to", to);
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

                                TableRow row = new TableRow();

                                TableCell imageCell = new TableCell();
                                TableCell imageCell2 = new TableCell();
                                TableCell pers = new TableCell();
                                int cek = event_type.IndexOf("Plate");

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
                                }
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





        [WebMethod]
        public static object GetDatta(string location, string from, string to)
        {

            // Lakukan logika untuk mengambil data berdasarkan parameter yang diterima
            // Misalnya, panggil layanan atau akses database
            List<object> cameraEvents = new List<object>();
            List<object> dataList = new List<object>();
            string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            DateTime fromDate;
            DateTime toDate;

            if (DateTime.TryParse(from.ToString(), out fromDate) && DateTime.TryParse(to, out toDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@from", from);
                        cmd.Parameters.AddWithValue("@to", to);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"]),
                                    // tambahkan inisialisasi properti lain yang sesuai dengan kolom-kolom data yang Anda ambil dari database
                                };

                                cameraEvents.Add(cameraEvent);

                            }
                        }
                    }
                }
            }
            else if (DateTime.TryParse(from.ToString(), out fromDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) >=@from", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@from", from);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            //List<object> dataList = new List<object>();
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"]),
                                    // tambahkan inisialisasi properti lain yang sesuai dengan kolom-kolom data yang Anda ambil dari database
                                };

                                cameraEvents.Add(cameraEvent);
                                var item = new { Name = dr["name"].ToString(), Value = dr["location"].ToString() };

                                // Tambahkan objek ke dalam koleksi
                                dataList.Add(item);
                            }
                            //string jsonData = JsonConvert.SerializeObject(dataList);

                            // Gunakan jsonData sesuai kebutuhan Anda, misalnya, kirimkan ke klien melalui WebMethod
                            //return jsonData;
                        }
                    }
                }
            }
            else if (DateTime.TryParse(to, out toDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) <=@to", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@to", to);
                        cmd.Prepare();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"]),
                                    // tambahkan inisialisasi properti lain yang sesuai dengan kolom-kolom data yang Anda ambil dari database
                                };

                                cameraEvents.Add(cameraEvent);
                            }
                        }
                    }
                }
            }

            // Contoh sederhana: Mengembalikan data JSON
            //var data = new[]
            //{
            //new { Name = "Item 1", Value = 10 },
            //new { Name = "Item 2", Value = 20 }
            //};

            return JsonConvert.SerializeObject(cameraEvents);

            //return data;
            //return cameraEvents;
            //string jsonData = JsonConvert.SerializeObject(dataList);
            //return jsonData;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetData(string location, string from, string to)
        {
            List<object> cameraEvents = new List<object>();
            string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

            DateTime fromDate;
            DateTime toDate;

            if (DateTime.TryParse(from, out fromDate) && DateTime.TryParse(to, out toDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@from", fromDate);
                        cmd.Parameters.AddWithValue("@to", toDate);
                        cmd.Prepare();

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"])
                                };

                                cameraEvents.Add(cameraEvent);
                            }
                        }
                    }
                }
            }
            else if (DateTime.TryParse(from, out fromDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) >=@from", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@from", fromDate);
                        cmd.Prepare();

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"])
                                };

                                cameraEvents.Add(cameraEvent);
                            }
                        }
                    }
                }
            }
            else if (DateTime.TryParse(from, out fromDate) && DateTime.TryParse(to, out toDate))
            {
                using (MySqlConnection con1 = new MySqlConnection(strcon))
                {
                    if (con1.State == ConnectionState.Closed)
                    {
                        con1.Open();
                    }

                    using (MySqlCommand cmd = new MySqlCommand("SELECT c.*,cam.location, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN cameras AS cam on c.camera_id=cam.name LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE cam.name =@camera AND DATE(c.occurred_at) <=@to", con1))
                    {
                        cmd.Parameters.AddWithValue("@camera", location);
                        cmd.Parameters.AddWithValue("@to", toDate);
                        cmd.Prepare();

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var cameraEvent = new
                                {
                                    Camera = dr["camera_id"].ToString(),
                                    OccurredAt = Convert.ToDateTime(dr["occurred_at"]),
                                    Location = dr["location"].ToString(),
                                    Name = dr["name"].ToString(),
                                    PersonType = dr["person_type"].ToString(),
                                    ExpiredAt = dr["expired_at"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["expired_at"])
                                };

                                cameraEvents.Add(cameraEvent);
                            }
                        }
                    }
                }
            }

            // Langsung kembalikan objek, ASP.NET akan mengonversinya menjadi JSON
            return cameraEvents;
        }



    }
}