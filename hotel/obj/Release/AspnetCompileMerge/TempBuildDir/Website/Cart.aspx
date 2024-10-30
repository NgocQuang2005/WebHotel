<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="hotel.Website.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .datebd {
            width: 100%;
            padding: 7px 17px;
            border: 1px solid #E0E0E2;
            color: #6C6C75;
        }

        .bookings {
            border: 1px solid black !important;
            border-radius: 5px !important;
        }

        .numpp {
            padding: 8px 22px !important;
        }

        .cart {
            color: #fff !important;
            padding: 15px 60px !important;
            font-weight: bold !important;
        }

        p {
            color: black !important;
        }

        .cart-container {
            width: 100%;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 0px !important;
        }

        .cart-header, .cart-item {
            display: flex;
            align-items: center;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .cart-header {
            background-color: #f8f8f8;
            font-weight: bold;
        }

        .cart-col {
            flex: 1;
            text-align: center;
        }

            .cart-col:first-child {
                flex: 0 0 40px;
            }

        .cart-item:last-child {
            border-bottom: none;
        }

        .delete-btn {
            background-color: #ff4d4f;
            color: #fff;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
            border-radius: 3px;
        }

            .delete-btn:hover {
                background-color: #ff7875;
            }

        .bookingroom {
            border-radius: 4px !important;
            width: 100px;
            font-size: 13px !important;
            height: 44px !important;
            line-height: 44px !important;
            float: right;
            margin: 0px !important;
        }

        .rl {
            border-right: 1px solid #ddd;
            border-left: 1px solid #ddd;
        }

        .bookss {
            right: 0;
            top: 58px;
        }
    </style>
    <script type="text/javascript">
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
    <div id="main-content" style="padding-bottom: 55px;">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Giỏ Hàng</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item"><a href="Room.aspx">Phòng</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Giỏ Hàng</li>
                    </ul>
                </div>
            </div>
        </div>

        <aside id="secondary" class="widget-area col-sm-12 col-md-12 container">
            <div class="wd wd-book-room row " style="margin: 40px 0px 20px 0px !important;">

                <div id="form-popup-room" class="form-popup-room ">
                    <div class="popup-container" style="border-radius: 5px;">
                        <a href="#" class="close-popup"><i class="ion-android-close"></i></a>
                        <form id="hotel-popup-results" name="hb-search-single-room" class="hotel-popup-results">
                            <div class="room-head">
                                <h3 class="room-title">Khách Sạn Lionn</h3>
                                <p class="description">Vui lòng nhập thông tin để hoàn thành việc đặt phòng này.</p>
                            </div>
                            <div class="search-room-popup">
                                <ul class="form-table clearfix">
                                    <li class="form-field">
                                        <p>Chọn Số Lượng Người</p>
                                        <asp:TextBox ID="txtNumOfPeople" runat="server" TextMode="Number" min="1" max="5" class="add w-100 numpp bookings" placeholder="Chọn Số Lượng Người"></asp:TextBox>
                                    </li>
                                    <li class="form-field">
                                        <p>Thời Gian Vào Phòng</p>
                                        <asp:TextBox ID="txtCheckIn" runat="server" class="check_in_date datebd bookings" TextMode="DateTimeLocal" placeholder="Arrival Date"></asp:TextBox>
                                    </li>
                                    <li class="form-field">
                                        <p>Thời Gian Trả Phòng</p>
                                        <asp:TextBox ID="txtCheckOut" runat="server" class="check_out_date datebd bookings" TextMode="DateTimeLocal" placeholder="Departure Date"></asp:TextBox>
                                    </li>
                                    <li class="form-field room-submit">
                                        <asp:Button Text="Đặt Ngay" OnClick="Datphong_Click" OnClientClick="return validateDates();" ID="btnSave" runat="server" CssClass="btn btn-outline-primary" />
                                    </li>
                                    <li class="form-field">
                                        <asp:Label ID="lblSuccess" runat="server" Visible="false" Text="" CssClass="success-message"></asp:Label>
                                    </li>
                                </ul>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="cart-container col-md-12" style="border-bottom: 0; border-radius: 5px 5px 0px 0px;">
                    <div class="cart-header">
                        <div class="cart-col">STT</div>
                        <div class="cart-col">ID Phòng</div>
                        <div class="cart-col">Tên Phòng</div>
                        <div class="cart-col">Số Lượng</div>
                        <div class="cart-col">Giá</div>
                        <div class="cart-col">Thao Tác</div>
                    </div>
                    <div id="itemPlaceholder" runat="server"></div>
                </div>
                <asp:ListView ID="lstViewCart" runat="server" DataKeyNames="ID" OnItemCommand="lstViewCart_ItemCommand" OnItemDeleting="lstViewCart_ItemDeleting">

                    <ItemTemplate>
                        <div class="cart-item col-md-12 rl">
                            <div class="cart-col"><%# Eval("STT") %></div>
                            <div class="cart-col"><%# Eval("ID") %></div>
                            <div class="cart-col"><%# Eval("TenPhong") %></div>
                            <div class="cart-col"><%# Eval("SL") %></div>
                            <div class="cart-col"><%# Eval("Gia") %>$</div>
                            <div class="cart-col ">
                                <asp:Button ID="btnDelete" runat="server" Text="Xóa" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Bạn có chắc muốn xóa?');" CssClass="delete-btn" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Text="" CssClass="success-message"></asp:Label>

                <div class="col-md-12" style="padding: 0px; margin-top: 20px;">
                    <a href="#" class="book-room bookingroom">Đặt Phòng Ngay</a>
                </div>
            </div>
        </aside>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
