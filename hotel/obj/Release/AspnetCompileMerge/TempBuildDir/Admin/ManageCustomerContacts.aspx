<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="ManageCustomerContacts.aspx.cs" Inherits="hotel.Admin.ManageCustomerContacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản lý liên hệ khách hàng</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">

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
            var aspTextBox = document.getElementById('<%= txtmt.ClientID %>');
            quill.root.innerHTML = aspTextBox.value;
            document.querySelector("form").onsubmit = function () {
                aspTextBox.value = quill.root.innerHTML;
            };
        });
        $(document).ready(function () {
            $('#<%= ddlCountries.ClientID %>').select2({
                placeholder: "Chọn Quốc Tịch ....",
                allowClear: true
            });
        });

    </script>
    <div class="container py-2">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px;">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Liên Hệ Khách Hàng"></asp:Label>
            </h2>
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtactive" runat="server" />
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Type</div>
                        <asp:TextBox ID="txttype" runat="server" placeholder=" Cus.." CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Tên Khách Hàng</div>
                        <asp:TextBox ID="txtname" runat="server" placeholder=" Nguyen Van ..." CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Giới Tính </div>
                        <asp:DropDownList ID="txtgtinh" CssClass="selc w-100" runat="server">
                            <asp:ListItem Text="Chọn Giới Tính" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Nữ" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Khác" Value="3"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Ngày Sinh</div>
                        <asp:TextBox ID="txtbthday" runat="server" TextMode="date" CssClass="crud_iput datetm"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Địa Chỉ</div>
                        <asp:TextBox ID="txtaddress" runat="server" placeholder="" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Email</div>
                        <asp:TextBox ID="txtemail" runat="server" MaxLength="300" placeholder=" abc123@gmail.com" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Số Điện Thoại</div>
                        <asp:TextBox ID="txtphone" runat="server" MaxLength="30" placeholder=" ****" CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Nhận Dạng Số</div>
                        <asp:TextBox ID="txtndphone" runat="server" placeholder=" +84.." CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Quốc Gia</div>
                        <asp:DropDownList ID="ddlCountries" runat="server" CssClass="crud_iput"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div id="editor">
                            <div class="formedit_lable">Miêu Tả</div>
                            <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                            <asp:TextBox ID="txtmt" runat="server" Style="display: none;" TextMode="MultiLine" CssClass="crud_iput w-100"></asp:TextBox>
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
            <h3 class="position-relative">Danh sách liên hệ khách hàng 
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <asp:TextBox ID="SearchName" runat="server" placeholder="Tìm Kiếm Khách Hàng ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewCustomDetails" OnItemCommand="lstViewCustomDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="165px"><b>Tên Khách Hàng</b></th>
                                    <th class="rb" width="60px"><b>Giới Tính</b></th>
                                    <th class="rb" width="150px"><b>Địa Chỉ</b></th>
                                    <th class="rb" width="140px"><b>Số Điện Thoại</b></th>
                                    <th class="rb" width="80px"><b>Quốc Tịch</b></th>
                                    <th class="rb"><b>Thông Tin Thêm</b></th>
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
                            <td class="trb"><%# Eval("name") %></td>
                            <td class="trb">
                                <%#(Eval("Gender") != DBNull.Value && int.TryParse(Eval("Gender").ToString(), out int genderValue) ? 
                              (genderValue == 1 ? "Nam" : 
                              (genderValue == 2 ? "Nữ" :
                              (genderValue == 3 ? "Khác" : "Chưa cập nhật"))) : "Chưa cập nhật") %>
                            </td>
                            <td class="trb"><%# Eval("address") %></td>
                            <td class="trb"><%# Eval("phone") %></td>
                            <td class="trb"><%# Eval("nationality") %></td>
                            <td class="trb"><%# Eval("description") %></td>
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
