<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ListOfReservations.aspx.cs" Inherits="hotel.Admin.ListOfReservations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách booking</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">


    <script type="text/javascript">
        //Phân Trang Cho Table

        function hideLabelAfterTimeout() {
            setTimeout(function () {
                var label = document.getElementById('<%= lblSuccess.ClientID %>');
                if (label) {
                    label.style.display = 'none';
                }
            }, 3000); // 5000 milliseconds = 5 seconds
        }
        <%--window.onload = function () {
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
        };--%>
        $(document).ready(function () {
            $('#<%= txtcustomer.ClientID %>').select2({
                placeholder: "Chọn Khách Hàng ....",
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchRoom.ClientID %>').select2({
                placeholder: "Chọn Phòng ....",
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= txtroom.ClientID %>').select2({
                placeholder: "Chọn Phòng ....",
                allowClear: true
            });
        });

    </script>
    <div class="container py-2">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout ">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Đơn Bookings"></asp:Label>
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
                        <div class="formedit_lable">Phòng</div>
                        <asp:DropDownList ID="txtroom" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Khách Hàng </div>
                        <asp:DropDownList ID="txtcustomer" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Số Lượng Người</div>
                        <asp:TextBox ID="txtnumofpp" runat="server" placeholder=" 2..." TextMode="Number" min="1" max="5" CssClass="crud_iput "></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Thời Gian Vào</div>
                        <asp:TextBox ID="txtCheckIn" runat="server" CssClass="crud_iput datetm" TextMode="DateTimeLocal" placeholder="Arrival Date"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Thời Gian Trả</div>
                        <asp:TextBox ID="txtCheckOut" runat="server" CssClass="crud_iput datetm" TextMode="DateTimeLocal" placeholder="Departure Date"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái</div>
                        <asp:DropDownList ID="drStatus" CssClass="selc w-100" runat="server">
                            <asp:ListItem Text="Trạng Thái Bookings.." Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Đang Chờ Xác Nhận" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Đã Xác Nhận" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Đã Vào Phòng" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Đã Trả Phòng" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Đã Hủy" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Đã Hết Hạn" Value="6"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái Thanh Toán</div>
                        <div class="stast">
                            <asp:CheckBox ID="txtthanhtoan" runat="server" />
                        </div>
                    </div>

                    <div class="formedit_div">
                        <asp:Label ID="lblSuccess" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
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

            <h3 class="position-relative">Danh Sách Bookings
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm Kiếm Thông Tin Bookings: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập số điện thoại khách hàng ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="STT Trạng Thái(All)" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchNguoiTao" CssClass="selc" runat="server">
                    <asp:ListItem Text="Người Tạo Đơn" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Nhân Viên" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Khách Hàng" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchRoom" runat="server" CssClass="selc mt-3"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">
                <asp:ListView ID="lstViewBookingDetails" OnItemCommand="lstViewBookingDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb"><b>Tên Khách hàng</b></th>
                                    <th class="rb" width="30px"><b>Mã Order</b></th>
                                    <th class="rb" width="100px"><b>Tên phòng</b></th>
                                    <th class="rb" width="20px"><b>Số Lượng</b></th>
                                    <th class="rb" width="70px"><b>Phone</b></th>
                                    <th class="rb" width="110px"><b>Giờ Vào Phòng</b></th>
                                    <th class="rb" width="110px"><b>Giờ Trả Phòng</b></th>
                                    <th class="rb" width="45px"><b>Thanh Toán</b></th>
                                    <th class="rb" width="145px"><b>STT Bookings</b></th>
                                    <th class="rb" width="105px"><b>Người Tạo</th>
                                    <th class="rb" width="75px"><b>Hành Động</th>
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
                            <td class="trb"><%# Eval("customer_name") %></td>
                            <td class="trb"><%# Eval("order_code") %></td>
                            <td class="trb"><%# Eval("room_name") %></td>
                            <td class="trb text-right"><%# Eval("nb_people") %> </td>
                            <td class="trb"><%# Eval("customer_phone") %></td>
                            <td class="trb"><%# Eval("check_in_time") %></td>
                            <td class="trb"><%# Eval("check_out_time") %></td>
                            <td class="trb"><%# (Eval("payment") != DBNull.Value && Convert.ToBoolean(Eval("payment")))?"<i style='color:green' class='fa fa-check' ></i>":"<i style='color:red'  class='fa fa-times'></i>" %></td>
                            <td class="trb">
                                <%#(Eval("trangthai") != DBNull.Value && int.TryParse(Eval("trangthai").ToString(), out int statusValue) ? 
                                  (statusValue == 1 ? "Đang Chờ Xác Nhận" : 
                                  (statusValue == 2 ? "Đã Xác Nhận" :
                                  (statusValue == 3 ? "Đã Vào Phòng" :
                                  (statusValue == 4 ? "Đã Trả Phòng" :
                                  (statusValue == 5 ? "Đã Hủy" :
                                  (statusValue == 6 ? "Đã Hết Hạn" : "Chưa cập nhật")))))) : "Chưa cập nhật") %>
                            </td>
                            <td class="trb"><%# Eval("nguoitao") %></td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("order_id") %>' runat="server"><i class="fa fa-edit"></i> Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("order_id") %>' runat="server"><i style="color:red" class="fa fa-trash"></i> Xóa</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <asp:Label ID="lblMessage" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
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
