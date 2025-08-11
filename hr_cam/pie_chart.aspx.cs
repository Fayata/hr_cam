using iText.Kernel.Pdf.Canvas.Parser;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace hr_cam
{
    public partial class pie_chart : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.TextMode = TextBoxMode.DateTimeLocal;
            TextBox2.TextMode = TextBoxMode.DateTimeLocal;
            if (!IsPostBack)
            {
                if (Session["camera_site_pie"] == null)
                {
                    string camera_site = "none";
                    Session["camera_site_pie"] = camera_site;

                }

                if (Session["from_pie"] != null)
                {
                }
                else
                {
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    Session["from_pie"] = today;

                }
                if (Session["to_pie"] != null)
                {
                    TextBox2.Text = Session["to_pie"].ToString();
                }
                TextBox1.Text = Session["from_pie"].ToString();
                DropDownList1.Items.Add(new ListItem("", "none"));
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
                                }
                            }
                            else
                            {
                            }
                        }
                    }

                }
                DropDownList1.SelectedValue = Session["camera_site_pie"].ToString();
                FillChart3();
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
            string camera_site = Session["camera_site_pie"].ToString();
            string from = Session["from_pie"].ToString();
            DateTime parsedDateTime = DateTime.Parse(from);
            string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string to = null;
            string to_date = null;
            //Response.Write("<script>alert('Isinya:" + Session["to_people"] +"')</script>");
            if (Session["to_pie"] != null && !string.IsNullOrEmpty(Session["to_pie"].ToString()))
            {
                //Response.Write("<script>alert('Ga kosong')</script>");
                to = Session["to_pie"].ToString();
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
                    string camera_name = "";
                    //DateTime fromDate;
                    //DateTime toDate;

                    using (MySqlConnection con1 = new MySqlConnection(strcon))
                    {
                        if (con1.State == ConnectionState.Closed)
                        {
                            con1.Open();
                        }
                        string query = @"SELECT c.id AS event_id,c.person_identification_number, c.occurred_at, c.type, cam.location, cam.name FROM camera_events c join cameras cam on c.camera_id=cam.name WHERE (c.image_file != '' or c.image_file != NULL) AND c.camera_id=@camera";
                        if (from_date !=null)
                        {
                            query += @" AND c.occurred_at >=@from";
                        }
                        if (to_date!=null)
                        {
                            query += @" AND c.occurred_at <=@to";
                        }
                        query += @" ORDER BY c.occurred_at";
                        //using (MySqlCommand cmd = new MySqlCommand("SELECT c.*, p.name, p.type as person_type, m.expired_at FROM hr_cam.camera_events AS c LEFT JOIN hr_cam.persons AS p ON c.person_identification_number = p.identification_number LEFT JOIN mcu.mcu AS m ON m.person_id = c.person_identification_number WHERE c.camera_id =@camera AND DATE(c.occurred_at) >=@from AND DATE(c.occurred_at) <=@to", con1))
                        using (MySqlCommand cmd = new MySqlCommand(query, con1))
                        {
                            cmd.Parameters.AddWithValue("@camera", camera);
                            if (from_date != null)
                            {
                                cmd.Parameters.AddWithValue("@from", from_date);
                            }
                            if (to_date!=null)
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
                                        satu++;
                                    }
                                    else if (type == "Face Recognition Not Match")
                                    {
                                        dua++;
                                    }
                                    else if (type == "Plate Match")
                                    {
                                        tiga++;
                                    }
                                    else if (type == "Plate MisMatch")
                                    {
                                        empat++;
                                    }

                                }
                                //int jum = fv + fi;
                                //int jum = nm + fi + bl;
                                int jum = satu+dua+tiga+empat;
                                jumlah.Add(jum);
                                
                            }
                        }
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
                ScriptManager.RegisterStartupScript(this, GetType(), "ChartScript", $"fillChart3({labelsJson},{jumlahJson});", true);
            }


        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string camera_site = DropDownList1.SelectedValue;
            string dari = TextBox1.Text;
            if (!DateTime.TryParse(dari, out _))
            {
                dari = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            string ke = TextBox2.Text;
            Session["camera_site_pie"] = camera_site;
            Session["from_pie"] = dari;
            Session["to_pie"] = ke;
            Response.Redirect("pie_chart.aspx");
        }
    }
}