<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="create_report.aspx.cs" Inherits="hr_cam.create_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .success-message {
            background-color: #DFF0D8; /* Warna background untuk pesan sukses */
            /* Tambahan gaya sesuai kebutuhan */
        }

        .error-message {
            background-color: #F2DEDE; /* Warna background untuk pesan gagal */
            /* Tambahan gaya sesuai kebutuhan */
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <div class="row">
            <div class="col-12">
                <div class="card my-4">
                    <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                        <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
                            <h5 class="text-white text-capitalize ps-3">Create Report</h5>
                        </div>
                    </div>
                    <div class="card-body px-0 pb-2">
                        <%--<p>
                            Output:
                            <asp:Label ID="lblOutput" runat="server"></asp:Label>
                        </p>--%>
                        <div class="row m-1">
                            <div class="col-sm-6">
                                <label>Site</label>
                                <div class="form-group form-group-custom mb-4">
                                    <select id="Select4" runat="server" class="js-example-basic-multiple" name="site[]" multiple="true" style="width: 100%;" onchange="cekubah()"></select>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Camera</label>
                                <div class="form-group form-group-custom mb-4">
                                    <select id="SelectMultiple" runat="server" class="js-example-basic-multiple" name="camera[]" multiple="true" style="width: 100%;"></select>
                                </div>
                            </div>

                        </div>
                        <div class="row m-1">
                            <div class="col-sm-6">
                                <label>Snapshot Type</label>
                                <div class="form-group form-group-custom mb-4">
                                    <select id="Select1" runat="server" class="js-example-basic-multiple" name="snapshot[]" multiple="true" style="width: 100%;"></select>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Image Type</label>
                                <div class="form-group form-group-custom mb-4">
                                    <asp:DropDownList id="Select5" runat="server" name="image_type" class="form-control" style="width: 100%;"></asp:DropDownList>
                                    <%--<select id="Select5" runat="server" name="image_type" class="form-control" style="width: 100%;"></select>--%>
                                    <%--<select id="Select5" runat="server" class="js-example-basic-multiple" name="image_type[]" multiple="true" style="width: 100%;"></select>--%>
                                </div>
                            </div>
                        </div>
                        <div class="row m-1">
                            <div class="col-sm-6">
                                <label>Person Status</label>
                                <div class="form-group form-group-custom mb-4">
                                    <select id="Select2" runat="server" class="js-example-basic-multiple" name="person_status[]" multiple="true" style="width: 100%;"></select>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label>Vehicle Status</label>
                                <div class="form-group form-group-custom mb-4">
                                    <select id="Select3" runat="server" class="js-example-basic-multiple" name="vehicle_status[]" multiple="true" style="width: 100%;"></select>
                                </div>
                            </div>
                        </div>
                        <div class="row m-1">
                            <div class="col-sm-6">
                                <label>Start Date</label>
                                <div class="form-group form-group-custom mb-4">
                                    <asp:TextBox ID="TextBox1" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label>End Date</label>
                                <div class="form-group form-group-custom mb-4">
                                    <asp:TextBox ID="TextBox2" runat="server" TextMode="DateTimeLocal" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row m-1">
                            <div class="col-sm-6">
                                <div class="form-group form-group-custom mb-4">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Unique Valid Vehicle" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-custom mb-4">
                                    <asp:CheckBox ID="CheckBox5" runat="server" Text="Unique Invalid Vehicle" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-custom mb-4">
                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="Unique Valid Person" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-custom mb-4">
                                    <asp:CheckBox ID="CheckBox3" runat="server" Text="Unique Invalid Person" />
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group form-group-custom mb-4">
                                    <asp:CheckBox ID="CheckBox4" runat="server" Text="Unique Blacklist Person" />
                                </div>
                            </div>
                        </div>
                        <div class="m-3">
                            <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Save" OnClick="Button1_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#DataTable').DataTable();
            $('.js-example-basic-multiple').select2();
        });

        function cekubah() {
            var selectedSites = $('#<%= Select4.ClientID %>').val();
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
            alert("masuk")
            // Mengambil nilai dari Select2
            var camera = $('#<%= SelectMultiple.ClientID %>').val();
            console.log("Camera:", camera);
            var type = $('#<%= Select1.ClientID %>').val();
            console.log("Type:", type);
            var person = $('#<%= Select2.ClientID %>').val();
            console.log("Person:", person);
            var vehicle = $('#<%= Select3.ClientID %>').val();
            console.log("Vehicle:", vehicle);
        }

    </script>
</asp:Content>
