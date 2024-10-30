﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="ManageWebsiteContent.aspx.cs" Inherits="hotel.Admin.ManageWebsiteContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản lý nội dung website</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
        //Phân Trang Cho Table

        function hideLabelAfterTimeout() {
            setTimeout(function () {
                var label = document.getElementById('<%= txtlableer.ClientID %>');
                if (label) {
                    label.style.display = 'none';
                }
            }, 5000); // 5000 milliseconds = 5 seconds
        }
        document.addEventListener("DOMContentLoaded", function () {
            // Initialize Quill editor
            const quill = new Quill('#quillEditor', {
                theme: 'snow'
            });

            // Reference to the ASP.NET TextBox control
            var aspTextBox = document.getElementById('<%= txtcontent.ClientID %>');

            // Set initial value from the ASP.NET TextBox to Quill editor
            quill.root.innerHTML = aspTextBox.value;

            // Sync Quill content to ASP.NET TextBox on form submit
            document.querySelector("form").onsubmit = function () {
                aspTextBox.value = quill.root.innerHTML;
            };
        });
        $(document).ready(function () {
            $('#<%= txtauthoremploy.ClientID %>').select2({
                placeholder: "Chọn Nhân Viên ....",
                allowClear: true
            });
        });

    </script>
    <div class="container">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px;">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Nội Dung WebSite"></asp:Label>
            </h2>
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtacti" runat="server" />
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Nhân Viên Đăng</div>
                        <asp:DropDownList ID="txtauthoremploy" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Tiêu Đề </div>
                        <asp:TextBox ID="txttitle" runat="server" CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Ngày Đăng</div>
                        <asp:TextBox ID="txtpdate" runat="server" TextMode="DateTimeLocal" CssClass="crud_iput datetm"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Từ Khóa</div>
                        <asp:TextBox ID="txtkeyword" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Trang , Chủ Đề</div>
                        <asp:DropDownList ID="txtcd" CssClass="selc w-100" runat="server">
                            <asp:ListItem Text="Chọn Chủ Đề , Trang" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Home" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Blog" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Event" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div id="editor">
                            <div class="formedit_lable">Ghi Chú</div>
                            <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                            <asp:TextBox ID="txtcontent" Style="display: none;" runat="server" TextMode="MultiLine" CssClass="crud_area w-100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="formedit_div">
                        <asp:Label ID="txtlableer" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="buttonEvent">
                    <asp:Button Text="Lưu" OnClick="btnSave_Click" ID="btnSave" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                    <asp:Button Text="Cập Nhật" ID="btnUpdate" OnClick="btnUpdate_Click" Visible="false" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                    <asp:Button Text="Hủy" ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-outline-danger"></asp:Button>
                </div>
            </div>

        </div>
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Danh Sách Nội Dung Website
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm bài viết: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập Từ Khóa ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchChude" CssClass="selc " runat="server">
                    <asp:ListItem Text="Chọn Chủ Đề , Trang" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Home" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Blog" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Event" Value="3"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewWebDetails" OnItemCommand="lstViewWebDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="170px"><b>Nhân Viên Đăng</b></th>
                                    <th class="rb" width="70px"><b>Ảnh</b></th>
                                    <th class="rb"><b>Tiêu Đề</b></th>
                                    <th class="rb" width="110px"><b>Ngày Đăng</b></th>
                                    <th class="rb" width="80px"><b>Từ Khóa</b></th>
                                    <th class="rb" width="75px"><b>Trang</b></th>
                                    <th class="b" width="150px"><b>Thao Tác</b></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="itemPlaceholder" runat="server"></tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# (Container.DataItemIndex + 1) % 2 == 0 ? "altrow" : "normalrow" %>'>
                            <td class="trb"><%# Container.DataItemIndex + 1 %></td>
                            <td class="trb"><%# (Eval("active") != DBNull.Value && Convert.ToBoolean(Eval("active")))?"<i style='color:green' class='fa fa-check' ></i>":"<i style='color:red'  class='fa fa-times'></i>" %></td>
                            <td class="trb"><%# Eval("namenv") %></td>
                            <td class="trb">
                                <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' alt="Ảnh tài liệu" style="max-width: 100px; max-height: 100px;" /></td>
                            <td class="trb text-center"><%# Eval("title") %></td>
                            <td class="trb"><%# Eval("publish_date") %></td>
                            <td class="trb"><%# Eval("keyword") %></td>
                            <td class="trb">
                                <%#(Eval("category") != DBNull.Value && int.TryParse(Eval("category").ToString(), out int CategoryValue) ? 
                              (CategoryValue == 1 ? "Home" : 
                              (CategoryValue == 2 ? "Blog" :
                              (CategoryValue == 3 ? "Event" : "Chưa cập nhật"))) : "Chưa cập nhật") %>
                            </td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i> Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color:red" class="fa fa-trash"></i> Xóa</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="pageNavPosition" class="text-right pointer"></div>
        </div>
    </div>
    <script type="text/javascript">
        var pager = new Pager('tblEmpDetails', 5);
        pager.init();
        pager.showPageNav('pager', 'pageNavPosition');
        pager.showPage(1);
    </script>
</asp:Content>
