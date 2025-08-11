using iText.Html2pdf;
//using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace hr_cam
{
    public partial class report_daily_updated_people : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["from_daily_updated_people"] != null)
                //{
                //}
                //else
                //{
                //    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                //    Session["from_daily_updated_people"] = today;

                //}
                //if (Session["to_daily_updated_people"] != null)
                //{
                //    TextBox2.Text = Session["to_daily_updated_people"].ToString();
                //}
                //TextBox1.Text = Session["from_daily_updated_people"].ToString();
                FillEventHistory();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //    //Response.Write("masuk semua apa ");
            //    //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //    dari = today;

            //}
            ////Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            //string ke = TextBox2.Text;
            //Session["from_daily_updated_people"] = dari;
            //Session["to_daily_updated_people"] = ke;
            Response.Redirect("report_daily_updated_people.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //    //Response.Write("masuk semua apa ");
            //    //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //    dari = today;

            //}
            ////Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            //string ke = TextBox2.Text;
            //Session["from_daily_updated_people"] = dari;
            //Session["to_daily_updated_people"] = ke;

            //Response.Redirect("report_daily_updated_people_pdf.aspx");
            // Mendapatkan markup HTML dari halaman ExportToPdf.aspx
            StringWriter sw = new StringWriter();
            Server.Execute("report_daily_updated_people_pdf.aspx", sw);
            string htmlContent = sw.ToString();

            // Membuat objek PdfWriter untuk menulis ke PDF
            string outputFilePath = Server.MapPath("~/Output.pdf");
            PdfWriter writer = new PdfWriter(outputFilePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

            // Menambahkan markup HTML ke PDF
            HtmlConverter.ConvertToPdf(htmlContent, pdf, new ConverterProperties());

            // Menutup dokumen PDF
            document.Close();

            // Mengarahkan pengguna untuk mengunduh file PDF yang dihasilkan
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Report Daily updated People.pdf");
            Response.TransmitFile(outputFilePath);
            Response.End();
        }

        private void FillEventHistory()
        {
            //string from = Session["from_daily_updated_people"].ToString();
            //DateTime parsedDateTime = DateTime.Parse(from);
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string to = null;
            //string to_date = null;
            ////Response.Write("<script>alert('Isinya:" + Session["to_daily_updated_people"] +"')</script>");
            //if (Session["to_daily_updated_people"].ToString() != "")
            //{
            //    //Response.Write("<script>alert('Ga kosong')</script>");
            //    to = Session["to_daily_updated_people"].ToString();
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

                string query = @"SELECT * from persons where date(updated_at)=@today order by updated_at ASC";

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
                                DateTime updatedAt = Convert.ToDateTime(dr["updated_at"]);
                                string updated_at = updatedAt.ToString("yyyy-MM-dd HH:mm:ss");
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
                                        ImageUrl = "person_image/" + image_file,
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
                                TableCell updated = new TableCell() { Text = updated_at };
                                //row.Cells.Add(no);
                                row.Cells.Add(imageCell);
                                row.Cells.Add(ic);
                                row.Cells.Add(nama);
                                row.Cells.Add(gendernya);
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            //string dari = TextBox1.Text;
            //DateTime fromDate;
            //if (DateTime.TryParse(dari.ToString(), out fromDate))
            //{
            //    //Response.Write("masuk semua apa ");
            //    //Response.Write("<script>alert('masuk semua apa " + dari + "')</script>");
            //}
            //else
            //{
            //    string today = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //    dari = today;

            //}
            ////Response.Write("<script>alert('akhirnya " + dari + "')</script>");
            //string ke = TextBox2.Text;
            //Session["from_daily_updated_people"] = dari;
            //Session["to_daily_updated_people"] = ke;
            //if (Session["camera_daily_updated_people"].ToString() == "none")
            //{
            //    Response.Write("<script>alert('Camera cannot be empty')</script>");
            //    //Response.Redirect("report_daily_updated_people.aspx");
            //}
            //else
            //{
            //ExportToExcelWithImages();
            ExportCSV();
            //}
        }

        protected void ExportCSV()
        {
            string fileName = "Report Daily Updated People.csv";

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "text/csv";
            string fullUrl = Request.Url.AbsoluteUri;
            string absolutePath = Request.Url.AbsolutePath;
            string baseUrl = fullUrl.Replace(absolutePath, "");
            // Siapkan header untuk CSV
            string csvContent = $"\"Report Daily Updated People\"\n";
            csvContent += "\"Image\",\"Identification Number\",\"Name\",\"Gender\",\"Type\",\"updated At\"\n";



            int x = 0;

            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = @"SELECT * from persons where date(updated_at)=@today order by updated_at ASC";

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
                                DateTime updatedAt = Convert.ToDateTime(dr["updated_at"]);
                                string updated_at = updatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string name = dr["name"].ToString();
                                string gender = dr["gender"].ToString();
                                string type = dr["type"].ToString();
                                string link1 = baseUrl + "/person_image/" + image_file;
                                csvContent += $"\"{link1}\",\"{identification_number}\",\"{name}\",\"{gender}\",\"{type}\",\"{updated_at}\"\n";

                            }
                            dr.Close();
                        }
                        else
                        {
                        }
                    }

                }

                Response.Output.Write(csvContent);
                Response.Flush();
                Response.End();
            }



        }


    }
}