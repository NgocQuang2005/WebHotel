<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="BookingOrders.aspx.cs" Inherits="hotel.Website.BookingOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đơn Đặt Phòng</title>
    <script>
        window.onload = function () {
            var today = new Date().toISOString().split('T')[0]; // Lấy ngày hiện tại dưới dạng yyyy-MM-dd
            var checkIn = document.getElementById('<%= txtCheckIn.ClientID %>');
            var checkOut = document.getElementById('<%= txtCheckOut.ClientID %>');

            checkIn.setAttribute('min', today + "T00:00"); // Thiết lập giá trị min cho trường Check-In
            checkOut.setAttribute('min', today + "T00:00"); // Thiết lập giá trị min cho trường Check-Out

            checkIn.addEventListener('change', function () {
                if (this.value) {
                    checkOut.setAttribute('min', this.value); // Cập nhật giá trị min của Check-Out khi Check-In thay đổi
                } else {
                    checkOut.setAttribute('min', today + "T00:00"); // Thiết lập lại giá trị min cho Check-Out nếu Check-In bị xóa
                }
            });
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Đơn Đặt Phòng</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Đơn Đặt Phòng </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="container">
            <div id="listbookings" runat="server">
                <h2 class="mt-4">Danh Sách Đặt Phòng</h2>
                <div class="table-responsive text-nowrap">
                    <asp:ListView ID="lstBookingDetails" runat="server">
                        <LayoutTemplate>
                            <table id="tblBookingDetails" class="table table-striped table-bordered">
                                <thead>
                                    <tr class=" text-white" style="background: #99cdd8;">
                                        <th class="text-center" width="30px"><b>STT</b></th>
                                        <th class="text-center" style="width: 150px;"><b>Mã Bookings</b></th>
                                        <th class="text-left" style="width: 200px;"><b>Tên Phòng</b></th>
                                        <th class="text-center" style="width: 150px;"><b>Số Lượng Người</b></th>
                                        <th class="text-center" style="width: 180px;"><b>Giờ Vào</b></th>
                                        <th class="text-center" style="width: 180px;"><b>Giờ Ra</b></th>
                                        <th class="text-center" style="width: 160px;"><b>Trạng Thái</b></th>
                                        <th class="text-center" style="width: 150px;"><b>Thao Tác</b></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr id="itemPlaceholder" runat="server"></tr>
                                </tbody>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class='<%# (Container.DataItemIndex + 1) % 2 == 0 ? "table-secondary" : "" %>'>
                                <td class="text-center"><%# Container.DataItemIndex + 1 %></td>
                                <td class="text-center"><%# Eval("order_code") %></td>
                                <td class="text-left"><%# Eval("room_name") %></td>
                                <td class="text-center"><%# Eval("nb_people") %></td>
                                <td class="text-center"><%# Eval("check_in_time", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                <td class="text-center"><%# Eval("check_out_time", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                <td class="text-center">
                                    <%#(Eval("trangthai") != DBNull.Value && int.TryParse(Eval("trangthai").ToString(), out int statusValue) ? 
                                      (statusValue == 1 ? "Đang Chờ Xác Nhận" : 
                                      (statusValue == 2 ? "Đã Xác Nhận" :
                                      (statusValue == 3 ? "Đã Vào Phòng" :
                                      (statusValue == 4 ? "Đã Trả Phòng" :
                                      (statusValue == 5 ? "Đã Hủy" :
                                      (statusValue == 6 ? "Đã Hết Hạn" : "Chưa cập nhật")))))) : "Chưa cập nhật") %>
                                </td>
                                <td class="text-center">
                                    <asp:Button ID="btnEdit" runat="server" Text="Sửa" CommandArgument='<%# Eval("order_id") %>' OnClick="btnEdit_Click" CssClass="btn btn-primary btn-sm" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Hủy" CommandArgument='<%# Eval("order_id") %>' OnClick="btnCancel_Click" CssClass="btn btn-danger btn-sm" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <div id="form_data" runat="server" visible="false">
                <h2 class="mt-4">Chỉnh Sửa Đặt Phòng</h2>
                <div class="form-group">
                    <label for="txtnumofpp">Number of People</label>
                    <asp:TextBox ID="txtnumofpp" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtCheckIn">Check In Time</label>
                    <asp:TextBox ID="txtCheckIn" TextMode="DateTimeLocal" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtCheckOut">Check Out Time</label>
                    <asp:TextBox ID="txtCheckOut" TextMode="DateTimeLocal" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnCancelUpdate" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancelUpdate_Click" />
                </div>
                <asp:Label ID="lblSuccess" runat="server" Visible="false" CssClass="text-danger"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
