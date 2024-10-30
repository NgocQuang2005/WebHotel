<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Current_stock_quantity.aspx.cs" Inherits="hotel.Admin.Current_stock_quantity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Số Lượng Hàng Tồn Kho Hiện Tại</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <div class="container py-2">
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Danh Sách Chi Tiết Hoạt Động Của Hàng
            </h3>
            <div class="search">
                <p>Tìm kiếm sản phẩm: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên sản phẩm ......" CssClass="search_input" />
                <%--<asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:DropDownList ID="SearchStock" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:DropDownList ID="SearchWarehouse" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewInventoryDetails" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb"><b>Tên Sản Phẩm</b></th>
                                    <th class="rb" width="350px"><b>Tên Kho</b></th>
                                    <th class="rb" width="80px"><b>Số lượng</b></th>
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
                            <td class="trb "><%# Eval("stock_name") %></td>
                            <td class="trb"><%# Eval("warehouse_name") %></td>
                            <td class="trb text-right"><%# Eval("CurrentQuantity") %></td>

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
