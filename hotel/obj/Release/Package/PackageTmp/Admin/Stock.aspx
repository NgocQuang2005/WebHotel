<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Stock.aspx.cs" Inherits="hotel.Admin.CheckInventoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Các Mặt Hàng</title>
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
            // Gọi hàm từ MasterPage và truyền vào ClientID của điều khiển server-side
            initializeQuill('quillEditor', '<%= txtnote.ClientID %>');
        });
        $(document).ready(function () {
            $('#<%= txtncc.ClientID %>').select2({
                placeholder: "Chọn Nhà Cung Cấp ....",
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchNcc.ClientID %>').select2({
                placeholder: "Chọn Nhà Cung Cấp ....",
                allowClear: true
            });
        });
    </script>
    <div class="container">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Mặt Hàng"></asp:Label>
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
                        <div class="formedit_lable">Tên Sản Phẩm</div>
                        <asp:TextBox ID="txtname" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Loại Sản Phẩm </div>
                        <asp:TextBox ID="txtcategory" runat="server" CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Giá Nhập Vào</div>
                        <asp:TextBox ID="txtnhap" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Giá Bán Ra</div>
                        <asp:TextBox ID="txtxuat" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Nhà Cung Cấp</div>
                        <asp:DropDownList ID="txtncc" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div id="editor">
                            <div class="formedit_lable">Ghi Chú</div>
                            <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                            <asp:TextBox ID="txtnote" Style="display: none;" runat="server" TextMode="MultiLine" CssClass="w-100"></asp:TextBox>
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
            <h3 class="position-relative">Danh Sách Các Mặt Hàng
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm hàng: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên hàng ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchNcc" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewStockInDetails" OnItemCommand="lstViewStockInDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="160px"><b>Tên Sản Phẩm </b></th>
                                    <th class="rb" width="160px"><b>Loại Sản Phẩm </b></th>
                                    <th class="rb" width="60px"><b>Giá Nhập </b></th>
                                    <th class="rb" width="60px"><b>Giá Bán </b></th>
                                    <th class="rb"><b>Nhà Cung Cấp </b></th>
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
                            <td class="trb text-center"><%# Eval("s_name") %></td>
                            <td class="trb text-center"><%# Eval("s_cate") %></td>
                            <td class="trb text-center"><%# Eval("gianhap") %></td>
                            <td class="trb text-center"><%# Eval("giaban") %></td>
                            <td class="trb"><%# Eval("tenncc") %></td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i> Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="LinkButton1" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color:red" class="fa fa-trash"></i> Xóa</asp:LinkButton>
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
