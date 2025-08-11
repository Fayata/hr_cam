<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master"
AutoEventWireup="true" CodeBehind="card_detail.aspx.cs"
Inherits="hr_cam.card_detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content
  ID="Content2"
  ContentPlaceHolderID="ContentPlaceHolder1"
  runat="server"
>
  <div class="container-fluid py-4">
    <div class="row">
      <div class="col-12">
        <div class="card my-4">
          <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
            <div
              class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2"
            >
              <h5 class="text-white text-capitalize ps-3">
                <asp:Label
                  ID="Judul"
                  runat="server"
                  CssClass="text-white text-capitalize ps-3"
                ></asp:Label>
              </h5>
            </div>
          </div>
            <div id="filter">
                <div class="row mb-5">
                <div class="col-md-6">
                    <table style="width: 100%;margin:10px;" aria-describedby="filter">
                        <tr>
                            <th>Site</th>
                            <th>Location</th>
                            <th>From</th>
                            <th></th>
                            <th>To</th>
                            <th></th>
                        </tr>
                        <tr>
                            <td><select id="Select1" runat="server" class="js-example-basic-multiple" name="site[]" multiple="true" onchange="cekubah()"></select></td>
                            <td><select id="SelectMultiple" runat="server" class="js-example-basic-multiple" name="camera[]" multiple="true"></select></td>
                            <td><asp:TextBox ID="TextBox1" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                            <td>-</td>
                            <td><asp:TextBox ID="TextBox2" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox></td>
                            <!--<td><button onclick="coba()" class="btn btn-success">Filter</button></td>-->
                            <td align="center"><asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Filter" OnClick="Button1_Click" /></td>
                        </tr>
                    </table>
                </div>
            </div>
            </div>

          <div class="card-body px-0 pb-2">
            <div class="table-responsive p-0" style="margin: 5px">
              <table
                id="DataTable"
                class="table table-striped table-bordered"
                aria-describedby="vms_list"
              >
                <%--
                <thead id="TableHead"></thead>
                --%>
                <thead id="TableHead">
                  <asp:Literal
                    ID="LiteralTableHead"
                    runat="server"
                  ></asp:Literal>
                  <%--
                  <th>Image</th>
                  <th></th>
                  <th>Occurred At</th>
                  <th>Location</th>
                  <th>Site</th>
                  <th>Person</th>
                  <th>Plate Number</th>
                  <th>Status</th>
                  --%>
                </thead>
                <tbody runat="server" id="TableBody"></tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <script type="text/javascript">
    $(document).ready(function () {
      //$('#table_users').DataTable();
      //$('#Table1').DataTable();
        $("#DataTable").DataTable();
        $('.js-example-basic-multiple').select2();
        var currentUrl = window.location.href;
        let baseUrl = currentUrl.split('?')[0];
        //alert(baseUrl);
        // Ambil parameter 'card' yang sudah ada di URL
        var urlParams = new URLSearchParams(window.location.search);
        var cardValue = urlParams.get('card'); 
        //alert(cardValue);
        var cari = cardValue.search("Card2.");
        if (cari==0) {
            document.getElementById('filter').style.display = "none";
        } else {
            document.getElementById('filter').style.display = "block";
        }
        //alert(cari);
    });
      function cekubah() {
          //alert("masuk");
          var selectedSites = $('#<%= Select1.ClientID %>').val();
          //alert(selectedSites);
        $('#<%= SelectMultiple.ClientID %>').val([]).trigger('change');

        if (selectedSites.length > 0) {
            //console.log("ada ko");
            selectedSites.forEach(function (siteId) {
                $('#<%= SelectMultiple.ClientID %> option').each(function () {
                    var cameraSiteId = $(this).data('siteid');
                    if (cameraSiteId == siteId) {
                        $(this).prop('selected', true);
                    }
                });
            });
              $('#<%= SelectMultiple.ClientID %>').trigger('change');
        }

      }

      function coba() {
          var currentUrl = window.location.href;
          let baseUrl = currentUrl.split('?')[0];
          //alert(baseUrl);
          // Ambil parameter 'card' yang sudah ada di URL
          var urlParams = new URLSearchParams(window.location.search);
          var cardValue = urlParams.get('card'); 
          var parameter1 = '';
          var parameter2 = '';
          var parameter3 = $('#<%= Select1.ClientID %>').val();
          var parameter4 = $('#<%= SelectMultiple.ClientID %>').val();

          // Buat URL untuk redirect dengan parameter
          var url = baseUrl+'?card=' + cardValue + '&start_date=' + parameter1 + '&end_date=' + parameter2 + '&sites=' + parameter3 + '&location=' + parameter4;

          // Lakukan redirect ke halaman dengan parameter yang ditambahkan
          //window.location.href = url;
          window.location.replace(url);
          //console.log(window.location.href);
          //setTimeout(function () {
          //    window.location.href = url;
          //}, 1000); // Menunggu 1 detik sebelum melakukan redirect

      }



  </script>
</asp:Content>
