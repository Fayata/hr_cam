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
    public partial class report_daily_created_vehicle : System.Web.UI.Page
    {
        readonly string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string UrlImage = ConfigurationManager.AppSettings["urlImage"];
        string UrlImagePerson = ConfigurationManager.AppSettings["urlImagePerson"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["from_daily_created_vehicle"] != null)
                //{
                //}
                //else
                //{
                //    string today = DateTime.Now.ToString("yyyy-MM-dd 00:00");
                //    Session["from_daily_created_vehicle"] = today;

                //}
                //if (Session["to_daily_created_vehicle"] != null)
                //{
                //    TextBox2.Text = Session["to_daily_created_vehicle"].ToString();
                //}
                //TextBox1.Text = Session["from_daily_created_vehicle"].ToString();
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
            //Session["from_daily_created_vehicle"] = dari;
            //Session["to_daily_created_vehicle"] = ke;
            Response.Redirect("report_daily_created_vehicle.aspx");
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
            //Session["from_daily_created_vehicle"] = dari;
            //Session["to_daily_created_vehicle"] = ke;

            //Response.Redirect("report_daily_created_vehicle_pdf.aspx");
            // Mendapatkan markup HTML dari halaman ExportToPdf.aspx
            StringWriter sw = new StringWriter();
            Server.Execute("report_daily_created_vehicle_pdf.aspx", sw);
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
            Response.AppendHeader("Content-Disposition", "attachment; filename=Report Daily Created Vehicle.pdf");
            Response.TransmitFile(outputFilePath);
            Response.End();
        }

        private void FillEventHistory()
        {
            //string from = Session["from_daily_created_vehicle"].ToString();
            //DateTime parsedDateTime = DateTime.Parse(from);
            //string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //string to = null;
            //string to_date = null;
            ////Response.Write("<script>alert('Isinya:" + Session["to_daily_created_vehicle"] +"')</script>");
            //if (Session["to_daily_created_vehicle"].ToString() != "")
            //{
            //    //Response.Write("<script>alert('Ga kosong')</script>");
            //    to = Session["to_daily_created_vehicle"].ToString();
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

                string query = @"SELECT * from vehicles where date(created_at)=@today order by created_at ASC";

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
                                DateTime createdAt = Convert.ToDateTime(dr["created_at"]);
                                string created_at = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string type = dr["type"].ToString();

                                TableRow row = new TableRow();
                                //TableCell no = new TableCell();
                                //no.Text = x.ToString();
                                TableCell no = new TableCell() { Text = x.ToString() };
                                TableCell plate = new TableCell() { Text = plate_number };
                                TableCell typenya = new TableCell() { Text = type };
                                TableCell created = new TableCell() { Text = created_at };
                                //row.Cells.Add(no);
                                row.Cells.Add(plate);
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
            //Session["from_daily_created_vehicle"] = dari;
            //Session["to_daily_created_vehicle"] = ke;
            //if (Session["camera_daily_created_vehicle"].ToString() == "none")
            //{
            //    Response.Write("<script>alert('Camera cannot be empty')</script>");
            //    //Response.Redirect("report_daily_created_vehicle.aspx");
            //}
            //else
            //{
            //ExportToExcelWithImages();
            ExportCSV();
            //}
        }

        protected void ExportCSV()
        {
            string fileName = "Report Daily Created Vehicle.csv";

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "text/csv";
            string fullUrl = Request.Url.AbsoluteUri;
            string absolutePath = Request.Url.AbsolutePath;
            string baseUrl = fullUrl.Replace(absolutePath, "");
            // Siapkan header untuk CSV
            string csvContent = $"\"Report Daily Created Vehicle\"\n";
            csvContent += "\"License Plate Number\",\"Type\",\"Created At\"\n";



            int x = 0;

            DateTime fromDate = DateTime.Now;
            string today = fromDate.ToString("yyyy-MM-dd");
            using (MySqlConnection con = new MySqlConnection(strcon))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                string query = @"SELECT * from vehicles where date(created_at)=@today order by created_at ASC";

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
                                DateTime createdAt = Convert.ToDateTime(dr["created_at"]);
                                string created_at = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
                                string type = dr["type"].ToString();
                                csvContent += $"\"{plate_number}\",\"{type}\",\"{created_at}\"\n";

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
        protected void ExportToExcelWithImages()
        {
            string fileName = "Report Vehicle.xls";
            string camera = Session["camera_daily_created_vehicle"].ToString();
            if (camera != "0")
            {
                int jumlahCamera = Convert.ToInt32(camera);
                string from = Session["from_daily_created_vehicle"].ToString();
                DateTime parsedDateTime = DateTime.Parse(from);
                string from_date = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string per1 = from_date.Substring(8, 2) + "-" + from_date.Substring(5, 2) + "-" + from_date.Substring(0, 4);
                string per2 = "";
                string to = null;
                string to_date = null;
                //Response.Write("<script>alert('Isinya:" + Session["to_daily_created_vehicle"] +"')</script>");
                if (Session["to_daily_created_vehicle"].ToString() != "")
                {
                    //Response.Write("<script>alert('Ga kosong')</script>");
                    to = Session["to_daily_created_vehicle"].ToString();
                    DateTime parsedDateTime2 = DateTime.Parse(to);
                    to_date = parsedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");
                    per2 += to_date.Substring(8, 2) + "-" + to_date.Substring(5, 2) + "-" + to_date.Substring(0, 4);
                }
                else
                {
                    using (MySqlConnection con = new MySqlConnection(strcon))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        string query = @"SELECT MAX(ce.occurred_at) as to_date, ce.camera_id, ce.type AS event_type,ce.plate_number_file, MIN(ce.occurred_at) AS occurred_at, ce.person_identification_number, ce.image_file, c.location, p.name as person, p.gender, p.type as person_type, p.image_file as person_image, cs.name AS site, ce.image_file, ce.plate_number_file, ph.entry_code  as status FROM camera_events as ce JOIN cameras as c ON ce.camera_id = c.name JOIN vehicles as p ON ce.person_identification_number = p.identification_number JOIN camera_sites AS cs ON c.camera_site_id=cs.id LEFT JOIN person_history ph ON p.id=ph.person_id AND ph.created_at = (SELECT MAX(ph2.created_at) FROM person_history ph2 WHERE ph2.person_id = p.id AND ph2.created_at <= ce.occurred_at)  WHERE (ce.image_file != '' OR ce.image_file != NULL) ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_daily_created_vehicle0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @" AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

                                    }
                                }
                                query += @")";
                            }
                        }
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @" AND ce.type like '%Face%' GROUP BY ce.person_identification_number ORDER BY ce.occurred_at DESC LIMIT 1";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_daily_created_vehicle0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_daily_created_vehicle" + y].ToString());
                                    }
                                }
                            }

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
                                        DateTime occurredAt = (DateTime)dr["to_date"];
                                        string formattedDateTime = occurredAt.ToString("yyyy-MM-dd HH:mm:ss");
                                        per2 += formattedDateTime.Substring(8, 2) + "-" + formattedDateTime.Substring(5, 2) + "-" + formattedDateTime.Substring(0, 4);
                                    }
                                }
                            }
                        }
                    }
                }
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                // Siapkan HTML Table yang valid
                string tableHtml = $@"<table border='1'>
                        <tr>
                            <td colspan='7' align='center'><b>Report Vehicle</b></td>
                        </tr>
                        <tr>
                            <td colspan='7' align='center'><b>Periode {per1} to {per2}</b></td>
                        </tr>
                        <tr>
                            <td><b>No</b></td>
                            <td><b>Image</b></td>
                            <td></td>
                            <td><b>First Occurred At</b></td>
                            <td><b>Location</b></td>
                            <td><b>Site</b></td>
                            <td><b>Person</b></td>
                        </tr>";


                int x = 0;

                using (MySqlConnection con = new MySqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (camera == "0")
                    {

                        //Response.Write("<script>alert('Camera Site cannot be empty')</script>");
                    }
                    else
                    {
                        string query = @"SELECT * from vehicles where date(created_at)=@today order by created_at ASC ";
                        if (jumlahCamera > 0)
                        {
                            if (Session["camera_daily_created_vehicle0"].ToString() == "all")
                            {
                                query += @" AND c.id in (SELECT id from cameras where name not like 'LPR%')";
                            }
                            else
                            {
                                query += @" AND (";
                                for (int y = 0; y < jumlahCamera; y++)
                                {
                                    if (y == 0)
                                    {
                                        query += $"c.id=@camera{y}";
                                    }
                                    else
                                    {
                                        query += " OR ";
                                        query += $"c.id=@camera{y}";

                                    }
                                }
                                query += @")";
                            }
                        }
                        if (from_date != null)
                        {
                            query += @" AND ce.occurred_at >= @from ";
                        }
                        if (to_date != null)
                        {
                            query += @" AND ce.occurred_at <= @to ";
                        }
                        query += @" AND ce.type like '%Face%' GROUP BY ce.person_identification_number ORDER BY ce.occurred_at ASC";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            if (jumlahCamera > 0)
                            {
                                if (Session["camera_daily_created_vehicle0"].ToString() == "all")
                                {

                                }
                                else
                                {
                                    for (int y = 0; y < jumlahCamera; y++)
                                    {
                                        cmd.Parameters.AddWithValue($"@camera{y}", Session["camera_daily_created_vehicle" + y].ToString());
                                    }
                                }
                            }

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
                                        x++;
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
                                        string entry_code = dr["status"].ToString();
                                        //string statusnya = "";
                                        //if (event_type == "Face Recognition Not Match")
                                        //{
                                        //    statusnya = "Unregister";
                                        //}
                                        //else
                                        //{
                                        //    if (dr["status"].ToString() == "True" || dr["status"].ToString() == "0")
                                        //    {
                                        //        statusnya = "Valid";
                                        //    }
                                        //    else
                                        //    {
                                        //        if (dr["status"].ToString() == "1" || dr["status"].ToString() == "2")
                                        //        {
                                        //            statusnya = "Invalid";
                                        //        }
                                        //    }
                                        //}
                                        tableHtml += $@"<tr>
                                                <td>{x}</td>";
                                        string path = UrlImage + image_file;
                                        tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path}' alt='Foto' width='150' height='150'/></center></td>";

                                        if (!string.IsNullOrEmpty(person) && !string.IsNullOrEmpty(person_image))
                                        {
                                            string path2 = UrlImagePerson + person_image;
                                            tableHtml += $@"<td style='width:155; height:155;'><center><img src='{path2}' alt='Foto' width='150' height='150'/></center></td>";
                                        }
                                        else
                                        {
                                            tableHtml += $@"<td style='width:155;height:155;'><center></center></td>";
                                        }

                                        tableHtml += $@"<td>{occurred_at}</td>
                                                <td>{location}</td>
                                                <td>{site}</td>";

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
                                                    entry += "Badge Expired";
                                                }
                                                else if (entry_code == "2")
                                                {
                                                    warna += "red";
                                                    entry += "FTW Rejected Medical / Expired";
                                                }
                                                else if (entry_code == "3")
                                                {
                                                    warna += "red";
                                                    entry += "Daily Checkup Failed";
                                                }
                                            }
                                            tableHtml += $@"<td><span style='color:{warna};'>[{char.ToUpper(person_type[0]) + person_type.Substring(1)}]</span> - {person} - <span style='color:{warna};'>{entry}</span></td>";
                                        }
                                        else
                                        {
                                            tableHtml += $@"<td>{person}</td>";
                                        }

                                        tableHtml += $@"</tr>";

                                    }
                                    dr.Close();
                                }
                                else
                                {
                                }
                            }

                        }

                        tableHtml += @"</table>";

                        // Menuliskan HTML ke response
                        Response.Output.Write(tableHtml);
                        Response.Flush();
                        Response.End();
                    }

                }
            }
        }

    }
}