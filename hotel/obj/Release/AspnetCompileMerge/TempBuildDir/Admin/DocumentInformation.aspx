<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DocumentInformation.aspx.cs" Inherits="hotel.Admin.DocumentInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Thông tin tài liệu</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        .imguser {
            
            width: 165px !important;
        }
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
        $(document).ready(function () {
            $('#<%= txtbelong.ClientID %>').select2({
                placeholder: 'Chọn Nhân Viên...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= txtrtype.ClientID %>').select2({
                placeholder: 'Chọn Loại Phòng...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= txtroom.ClientID %>').select2({
                placeholder: 'Chọn Phòng...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= txtweb.ClientID %>').select2({
                placeholder: 'Chọn Tiêu Đề Web...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchEmployee.ClientID %>').select2({
                placeholder: 'Chọn Nhân Viên...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchTypeRoom.ClientID %>').select2({
                placeholder: 'Chọn Loại Phòng...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchRoom.ClientID %>').select2({
                placeholder: 'Chọn Phòng...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchWebsite.ClientID %>').select2({
                placeholder: 'Chọn Tiêu Đề Web...',
                allowClear: true
            });
        });
        function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgProfile.ClientID %>').src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <div class="container py-2">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Tài Liệu"></asp:Label>
            </h2>
            <div style="display: flex; gap: 1.6rem !important; align-items: center; margin: 20px;">
                <img id="imgProfile" runat="server" src="~/App_Themes/Admin_Pages/Layout/assets/img/avatars/avt.png" class="imguser" />
                <div>
                    <asp:FileUpload ID="FileUploadImage" runat="server" CssClass="" />
                </div>
            </div>
            <hr style="border-bottom: 1px solid #e6e6e6" />
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtacti" runat="server" />
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Type</div>
                        <asp:TextBox ID="txttype" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Nhân Viên </div>
                        <asp:DropDownList ID="txtbelong" CssClass="selc w-100" runat="server"></asp:DropDownList>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Websiteh </div>
                        <asp:DropDownList ID="txtweb" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Loại Phòng</div>
                        <asp:DropDownList ID="txtrtype" CssClass="selc w-100" runat="server"></asp:DropDownList>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Phòng</div>
                        <asp:DropDownList ID="txtroom" CssClass="selc w-100" runat="server"></asp:DropDownList>
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
            <asp:Label ID="lblErrorMessage" Font-Bold="true" Visible="false" runat="server"></asp:Label>

            
        </div>
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Tài Liệu
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm Loại tài liệu: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên loại tài liệu ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchTypeRoom" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:DropDownList ID="SearchEmployee" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:DropDownList ID="SearchWebsite" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:DropDownList ID="SearchRoom" runat="server" CssClass="selc mt-3"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewDocumentDetails" OnItemCommand="lstViewDocumentDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb"><b>Tài Liệu</b></th>
                                    <th class="rb" width="120px"><b>URL</b></th>
                                    <th class="rb" width="175px"><b>Nhân Viên </b></th>
                                    <th class="rb" width="120px"><b>Website </b></th>
                                    <th class="rb" width="135px"><b>Loại Phòng</b></th>
                                    <th class="rb" width="110px"><b>Phòng </b></th>
                                    <th class="b" width="155px"><b>Thao Tác</b></th>
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
                            <td class="trb text-center"><%# Eval("type") %></td>
                            <td class="trb">
                                <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' alt="Ảnh tài liệu" style="max-width: 100px; max-height: 100px;" />
                            </td>
                            <td class="trb"><%# Eval("tennv") %></td>
                            <td class="trb"><%# Eval("websitetitle") %></td>
                            <td class="trb"><%# Eval("tenloaiphong") %></td>
                            <td class="trb"><%# Eval("tenphong") %></td>
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
