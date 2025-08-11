//using iText.Layout.Element;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class report_daily_created_people_pdf : System.Web.UI.Page
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
            //string from = Session["from_daily_created_people"].ToString();
            //DateTime parsedDateTime = DateTime.Parse(from);
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string to = null;
            //string to_date = null;
            ////Response.Write("<script>alert('Isinya:" + Session["to_daily_created_people"] +"')</script>");
            //if (Session["to_daily_created_people"].ToString() != "")
            //{
            //    //Response.Write("<script>alert('Ga kosong')</script>");
            //    to = Session["to_daily_created_people"].ToString();
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

                string query = @"SELECT * from persons where date(created_at)=@today order by created_at ASC";

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

                                string image_file = dr["image_file"].ToString();
                                string identification_number = dr["identification_number"].ToString();
                                DateTime createdAt = Convert.ToDateTime(dr["created_at"]);
                                string created_at = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string name = dr["name"].ToString();
                                string gender = dr["gender"].ToString();
                                string type = dr["type"].ToString();

                                TableRow row = new TableRow();
                                //TableCell no = new TableCell();
                                //no.Text = x.ToString();
                                TableCell no = new TableCell() { Text = x.ToString() };
                                TableCell imageCell = new TableCell();

                                if (!string.IsNullOrEmpty(image_file))
                                {
                                    System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image
                                    {
                                        ImageUrl = UrlImagePerson + image_file,
                                        AlternateText = "icon title"
                                    };
                                    img.Style.Add("width", "150px");
                                    imageCell.Controls.Add(img);
                                }
                                else
                                {
                                    imageCell.Text = "";
                                }

                                imageCell.HorizontalAlign = HorizontalAlign.Center;
                                TableCell ic = new TableCell() { Text = identification_number };
                                TableCell nama = new TableCell() { Text = name };
                                TableCell gendernya = new TableCell() { Text = gender };
                                TableCell typenya = new TableCell() { Text = type };
                                TableCell created = new TableCell() { Text = created_at };
                                row.Cells.Add(no);
                                row.Cells.Add(imageCell);
                                row.Cells.Add(ic);
                                row.Cells.Add(nama);
                                row.Cells.Add(gendernya);
                                row.Cells.Add(typenya);
                                row.Cells.Add(created);
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