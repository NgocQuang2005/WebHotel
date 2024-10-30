<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CrudEmployee.aspx.cs" Inherits="hotel.Admin.CrudEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Crud Employee</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
        //Phân Trang Cho Table

        function hidedivAfterTimeout() {
            setTimeout(function () {
                var div = document.getElementById('<%= txtlableer.ClientID %>');
                if (div) {
                    div.style.display = 'none';
                }
            }, 5000); // 5000 milliseconds = 5 seconds
        }
        $(document).ready(function () {
            $('#<%= txtdepar.ClientID %>').select2({
                placeholder: 'Chọn Chi Nhánh...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchOrgstruc.ClientID %>').select2({
                placeholder: 'Chọn Chi Nhánh...',
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
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout " >
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Nhân Viên Mới"></asp:Label>
            </h2>
            <div style="display: flex; gap: 1.6rem !important; align-items: center; margin: 20px;">
                <img id="imgProfile" runat="server" src="~/App_Themes/Admin_Pages/Layout/assets/img/avatars/avt.png" class="imguser" />
                <div>
                    <asp:FileUpload ID="FileUploadImage" runat="server" onchange="previewImage(this);" />

                </div>
            </div>
            <hr style="border-bottom: 1px solid #e6e6e6" />
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div  class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtacti" runat="server" />
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable">Tên Nhân Viên</div>
                        <asp:TextBox ID="txtname" MaxLength="100" runat="server" placeholder=" Họ và Tên ..." CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div  class="formedit_lable">Giới Tính </div>
                        <asp:DropDownList ID="txtgtinh" CssClass="selc w-100" runat="server">
                            <asp:ListItem Text="Chọn Giới Tính" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Nữ" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Khác" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable">Ngày Sinh </div>
                        <asp:TextBox ID="txtbthday" runat="server" TextMode="Date" CssClass="crud_iput datetm"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div  class="formedit_lable">Số Điện Thoại</div>
                        <asp:TextBox ID="txtphone" MaxLength="20" runat="server" placeholder=" +84 ..." CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable">Email</div>
                        <asp:TextBox ID="txtemail" MaxLength="255" placeholder=" abc123@gmail.com " pattern="/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/" TextMode="Email" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div  class="formedit_lable">Địa Chỉ</div>
                        <asp:TextBox ID="txtaddress" MaxLength="255" runat="server" placeholder=" Địa Chỉ ..." CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable">Thành Phố</div>
                        <asp:TextBox ID="txtcity" runat="server" CssClass="crud_iput" placeholder=" Thành Phố ..."></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div  class="formedit_lable">Bắt Đầu Làm</div>
                        <asp:TextBox ID="txtstarwork" runat="server" TextMode="Date"  CssClass="crud_iput datetm"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable ">kết Thúc</div>
                        <asp:TextBox ID="txtenwork" runat="server" TextMode="Date" CssClass="crud_iput datetm"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div  class="formedit_lable">Chi Nhánh</div>
                        <asp:DropDownList ID="txtdepar" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div  class="formedit_lable ">Thông Báo</div>
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
            <h3 class="position-relative">Thông Tin Nhân Viên
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm nhân viên: </p>

                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên nhân viên ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động " Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchOrgstruc" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewEmployeeDetails" OnItemCommand="lstViewEmployeeDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="20px"><b>STT</b></th>
                                    <th class="rb" width="30px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="70px"><b>Ảnh</b></th>
                                    <th class="rb" width="160px"><b>Tên Nhân Viên</b></th>
                                    <th class="rb" width="60px"><b>Giới Tính</b></th>
                                    <th class="rb" width="70px"><b>Ngày Sinh</b></th>
                                    <th class="rb" width="85px"><b>Số Điện Thoại</b></th>
                                    <th class="rb"><b>Chi Nhánh</b></th>
                                    <th class="b" width="130px"><b>Thao Tác</b></th>
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
                            <td class="trb">
                                <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' alt="Ảnh tài liệu" style="max-width: 100px; max-height: 100px;" />
                            </td>
                            <td class="trb"><%# Eval("name") %></td>
                            <td class="trb">
                                <%#(Eval("Gender") != DBNull.Value && int.TryParse(Eval("Gender").ToString(), out int genderValue) ? 
                              (genderValue == 1 ? "Nam" : 
                              (genderValue == 2 ? "Nữ" :
                              (genderValue == 3 ? "Khác" : "Chưa cập nhật"))) : "Chưa cập nhật") %>
                            </td>
                            <td class="trb"><%# Eval("birthday") != DBNull.Value?Convert.ToDateTime(Eval("birthday")).ToString("dd/MM/yyyy"):"-.-" %></td>
                            <td class="trb"><%# Eval("phone") %></td>
                            <td class="trb"><%# Eval("tenchinhanh") %></td>
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
