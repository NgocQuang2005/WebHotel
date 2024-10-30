<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="hotel.Admin.EmployeeAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Employee Account
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        .showpass {
            color: black;
            position: absolute;
            right: 18px;
            top: 5px;
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
        function togglePassword() {
            var passwordField = document.getElementById('<%= txtmatkhau.ClientID %>');
            var toggleButton = document.getElementById('<%= btnShowPass.ClientID %>').firstElementChild;

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleButton.className = "fas fa-eye-slash";
            } else {
                passwordField.type = "password";
                toggleButton.className = "fas fa-eye";
            }
        }
        $(document).ready(function () {
            $('#<%= txtvaitro.ClientID %>').select2({
            });
        });
        $(document).ready(function () {
            $('#<%= txtemployee.ClientID %>').select2({
                placeholder: 'Chọn Nhân Viên...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= txtcustomer.ClientID %>').select2({
                placeholder: 'Chọn Khách Hàng...',
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
            $('#<%= SearchCustomer.ClientID %>').select2({
                placeholder: 'Chọn Khách Hàng...',
                allowClear: true
            });
        });
    </script>
    <div class="container">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px;">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm tài khoản nhân viên và khách hàng"></asp:Label>
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
                        <div class="formedit_lable">Vai Trò</div>
                        <asp:DropDownList ID="txtvaitro" CssClass="selc " style="max-width: 450px; width: 100%;"
                            runat="server">
                            <asp:ListItem Text="Chọn Vai Trò.." Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Quản Lý" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Nhân Viên" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Khách Hàng" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Nhân Viên </div>
                        <asp:DropDownList ID="txtemployee" runat="server" CssClass="selc " style="max-width: 450px; width: 100%;"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Khách Hàng</div>
                        <asp:DropDownList ID="txtcustomer" runat="server" CssClass="selc " style="max-width: 450px; width: 100%;"></asp:DropDownList>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Tên Đăng Nhập</div>
                        <asp:TextBox ID="txttendangnhap" placeholder=" NgocQuang14" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Mật Khẩu</div>
                        <div class="position-relative">
                            <asp:TextBox ID="txtmatkhau" runat="server" placeholder=" *****" TextMode="Password" CssClass="crud_iput "></asp:TextBox>
                            <asp:LinkButton ID="btnShowPass" runat="server" CssClass="showpass" OnClientClick="togglePassword(); return false;">
                                <i class="fas fa-eye"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable"></div>
                        <asp:Label ID="lblMatKhauError" runat="server" ForeColor="Red" CssClass="error"></asp:Label>
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
            <h3 class="position-relative">Tài khoản nhân viên và khách hàng
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm nhân viên: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên nhân viên hoặc khách hàng ......" CssClass="search_input" Width="358px" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchEmployee" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:DropDownList ID="SearchCustomer" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr " Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">
                <asp:ListView ID="lstViewAccountDetails" OnItemCommand="lstViewAccountDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="20px"><b>STT</b></th>
                                    <th class="rb" width="30px"><b>Trạng Thái</b></th>
                                    <th class="rb"><b>Tên Nhân Viên</b></th>
                                    <th class="rb" width="190px"><b>Tên Khách Hàng</b></th>
                                    <th class="rb" width="90px"><b>Tên Đăng Nhập</b></th>
                                    <th class="rb" width="110px"><b>Vai Trò</b></th>
                                    <th class="rb" width="130px"><b>Thao Tác</b></th>
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
                            <td class="trb text-center"><%# Eval("name") %></td>
                            <td class="trb text-center"><%# Eval("tenkhachhang") %></td>
                            <%--<td class="trb text-center"><%# Eval("password") %></td>--%>

                            <td class="trb"><%# Eval("username") %></td>
                            <td class="trb">
                                <%#(Eval("role") != DBNull.Value && int.TryParse(Eval("role").ToString(), out int roleValue) ? 
                              (roleValue == 1 ? "Quản Lý" : 
                              (roleValue == 2 ? "Nhân Viên" :
                              (roleValue == 3 ? "Khách Hàng" : "Chưa cập nhật"))) : "Chưa cập nhật") %>
                            </td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i>Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color: red" class="fa fa-trash"></i>Xóa</asp:LinkButton>
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
