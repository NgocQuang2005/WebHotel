<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="hotel.Admin.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Thanh Toán</title>
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
    </script>
    <div class="container py-2">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Danh Sách Thanh Toán"></asp:Label>
            </h2>
            <%--<asp:LinkButton ID="btnback" CssClass="btn btn-outline-secondary m-2" Visible="false" runat="server" OnClick="btnback_Click"><i class="fas fa-chevron-left"></i></asp:LinkButton>--%>
            <div>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <b>Mã Thanh Toán :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtcode" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Trạng Thái :</b>
                            </td>
                            <td class="stast">
                                <asp:CheckBox ID="txtacti" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Khách Hàng Order :</b>
                            </td>
                            <td>
                                <asp:DropDownList ID="txtodercus" CssClass="selc w-100" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Giảm Giá :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtdscou" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>% Giảm Giá  :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtdcpct" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Tổng Tiền :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtgrandtotal" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Ngày Thanh Toán :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpmdate" runat="server" TextMode="DateTimeLocal" CssClass="crud_iput datetm"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Phương Thức Thanh Toán :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpmmethod" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Mã Giao Dịch :</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txttrancode" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="txtlableer" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button Text="Lưu" OnClick="btnSave_Click" ID="btnSave" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                                <asp:Button Text="Cập Nhật" ID="btnUpdate" OnClick="btnUpdate_Click" Visible="false" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                                <asp:Button Text="Hủy" ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-outline-danger"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Danh Sách Thanh Toán
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm Số Điện Thoại Khách: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập Số Điện Thoại Khách Hàng ......" CssClass="search_input" />
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

                <asp:ListView ID="lstViewPayDetails" OnItemCommand="lstViewPayDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="160px"><b>Tên KH</b></th>
                                    <th class="rb" width="120px"><b>Số Điện Thoại</b></th>
                                    <th class="rb" width="70px"><b>O_Code</b></th>
                                    <th class="rb" width="50px"><b>Tổng Tiền</b></th>
                                    <th class="rb" width="150px"><b>TT Thanh Toán</b></th>
                                    <th class="rb"><b>Thanh Toán Ngày</b></th>
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
                            <td class="trb"><%# Eval("cm_name") %></td>
                            <td class="trb"><%# Eval("cm_phone") %></td>
                            <td class="trb"><%# Eval("o_code") %></td>
                            <td class="trb"><%# Eval("p_gtotal") %></td>
                            <td class="trb"><%# (Eval("o_pm_status") != DBNull.Value && Convert.ToBoolean(Eval("o_pm_status")))?"Đã Thanh Toán":"Chưa Thanh Toán" %></td>
                            <td class="trb text-center"><%# Eval("p_pdate") %></td>
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
