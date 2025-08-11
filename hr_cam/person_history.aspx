<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="person_history.aspx.cs" Inherits="hr_cam.person_history" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid py-4">
        <div class="row">
            <div class="col-12">
                <div class="card my-4">
                    <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
                        <div class="bg-gradient-primary shadow-primary border-radius-lg pt-3 pb-2">
                            <h5 class="text-white text-capitalize ps-3">Person History</h5>
                        </div>
                    </div>

                    <div class="card-body px-0 pb-2">
                        <div class="table-responsive p-3">
                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th style="width: 3%;">No</th>
                                        <th style="width: 12%;">Badge Number</th>
                                        <th style="width: 13%;">Name</th>
                                        <th style="width: 20%;">Status</th>
                                        <th style="width: 40%;">Note</th>
                                        <th style="width: 12%;">Date</th>
                                    </tr>
                                </thead>
                                <tbody runat="server" id="TableBody">
                                </tbody>
                            </table>
                        </div>

                        <!-- Pagination -->
                        <div class="d-flex justify-content-end mt-3 me-3">
                            <ul class="pagination mb-0">
                                <asp:Repeater ID="rptPager" runat="server">
                                    <ItemTemplate>
                                        <li class='<%# Eval("CssClass") %>'>
                                            <asp:LinkButton ID="lnkPage" runat="server" CssClass="page-link"
                                                Text='<%# Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                OnClick="Page_Changed" />
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
