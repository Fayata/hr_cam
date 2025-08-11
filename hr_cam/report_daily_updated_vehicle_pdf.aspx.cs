//using iText.Layout.Element;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class report_daily_updated_vehicle_pdf : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        string UrlLogo = ConfigurationManager.AppSettings["urlLogo"];
        protected void Page_Load(object sender, EventArgs e)
        {
            string imagePath = UrlLogo + "pertamina_logo2.png";
            string imagePath2 = UrlLogo + "logo.png";

            // Membaca gambar dari file lokal menjadi array byte
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            byte[] imageBytes2 = File.ReadAllBytes(imagePath2);

            // Mengonversi array byte gambar ke format base64
            string base64String = Convert.ToBase64String(imageBytes);
            string base64String2 = Convert.ToBase64String(imageBytes2);

            // Membuat string untuk src image dengan format base64
            string srcBase64 = $"data:image/jpeg;base64,{base64String}";
            string src2Base64 = $"data:image/jpeg;base64,{base64String2}";

            // Menetapkan nilai src dari elemen img di code.aspx
            logonya.Attributes["src"] = srcBase64;
            logo2.Attributes["src"] = src2Base64;
            if (!IsPostBack)
            {
                //Response.Write("camera site:"+Session["site_dpo"].ToString());
                //Response.Write("camera:"+Session["camera"].ToString());
                //Response.Write("from_dpo:"+Session["from_dpo"].ToString());
                //Response.Write("to_dpo:"+Session["to_dpo"].ToString());
                FillEventHistory();
            }
        }


        private void FillEventHistory()
        {
            //string from = Session["from_daily_updated_vehicle"].ToString();
            //DateTime parsedDateTime = DateTime.Parse(from);
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string to = null;
            //string to_date = null;
            ////Response.Write("<script>alert('Isinya:" + Session["to_daily_updated_vehicle"] +"')</script>");
            //if (Session["to_daily_updated_vehicle"].ToString() != "")
            //{
            //    //Response.Write("<script>alert('Ga kosong')</script>");
            //    to = Session["to_daily_updated_vehicle"].ToString();
            //    DateTime parsedDateTime2 = DateTime.Parse(to);
            //    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            //else
            //{
            //    //Response.Write("<script>alert('kosong')</script>");

            //}

            //    DateTime fromDate;
            //DateTime toDate;
            int x = 0;
            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = @"SELECT * from vehicles where date(updated_at)=@today order by updated_at ASC";

                //if (from_date != null)
                //{
                //    query += @" AND ce.occurred_at >= @from ";
                //}
                //if (to_date != null)
                //{
                //    query += @" AND ce.occurred_at <= @to ";
                //}
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    // if (from_date != null)
                    // {
                    //     cmd.Parameters.AddWithValue("@from", from_date);
                    // }
                    // if (to_date != null)
                    // {
                    //     cmd.Parameters.AddWithValue("@to", to_date);
                    // }
                    cmd.Parameters.AddWithValue("@today", today);
                    cmd.Prepare();
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                x++;

                                string plate_number = dr["plate_number"].ToString();
                                DateTime updatedAt = Convert.ToDateTime(dr["updated_at"]);
                                string updated_at = updatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string type = dr["type"].ToString();
                                TableRow row = new TableRow();
                                //TableCell no = new TableCell();
                                //no.Text = x.ToString();
                                TableCell no = new TableCell() { Text = x.ToString() };

                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell typenya = new TableCell() { Text = type };
                                TableCell updated = new TableCell() { Text = updated_at };
                                row.Cells.Add(no);
                                row.Cells.Add(plate);
                                row.Cells.Add(typenya);
                                row.Cells.Add(updated);
                                //row.Cells.Add(status);



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
}