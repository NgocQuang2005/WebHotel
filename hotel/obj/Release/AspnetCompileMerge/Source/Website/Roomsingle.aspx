<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Roomsingle.aspx.cs" Inherits="hotel.Website.Roomsingle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Phòng chi tiết</title>

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

        .cart {
            color: #fff !important;
            padding: 15px 60px !important;
            font-weight: bold !important;
        }

        label {
            color: black !important;
        }

        .bookingroom {
            border-radius: 4px !important;
            width: 271px;
            font-size: 14px !important;
            height: 55px !important;
            line-height: 55px !important;
            /* float: right; */
            margin: 0px !important;
        }

        .numpp {
            padding: 8px 22px !important;
        }



        .slider-container {
            position: relative;
            overflow: hidden;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            margin: auto;
        }

        .slider-wrapper {
            display: flex;
            transition: transform 0.5s ease-in-out;
        }

        .slider-image {
            min-width: 100%;
            transition: opacity 1s ease-in-out;
            opacity: 0;
            position: absolute; /* Ensure all images stack on top of each other */
            top: 0;
            left: 0;
            width: 100%;
            height: auto;
        }

            .slider-image.active {
                opacity: 1;
                position: relative; /* Position image normally when active */
            }

        .slider-button {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            background-color: rgba(0, 0, 0, 0.5);
            color: white;
            border: none;
            padding: 10px 18px;
            cursor: pointer;
            border-radius: 50%;
            z-index: 100; /* Ensure buttons are above the images */
        }

        .prev-button {
            left: 10px;
        }

        .next-button {
            right: 10px;
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
        document.addEventListener('DOMContentLoaded', () => {
            let slideIndex = 0;
            const slides = document.querySelectorAll('.slider-image');
            const totalSlides = slides.length;

            function showSlide(index) {
                slides.forEach((slide, i) => {
                    slide.classList.remove('active');
                    if (i === index) {
                        slide.classList.add('active');
                    }
                });
            }

            function changeSlide(step) {
                slideIndex = (slideIndex + step + totalSlides) % totalSlides;
                showSlide(slideIndex);
            }

            function autoSlide() {
                changeSlide(1);
                setTimeout(autoSlide, 5000);
            }

            // Initialize the slider
            showSlide(slideIndex);
            let autoSlideTimeout = setTimeout(autoSlide, 5000);

            // Attach event listeners to buttons
            document.querySelector('.prev-button').addEventListener('click', (event) => {
                event.preventDefault(); // Prevent default action
                changeSlide(-1);
                // Reset auto-slide timer
                clearTimeout(autoSlideTimeout);
                autoSlideTimeout = setTimeout(autoSlide, 5000);
            });

            document.querySelector('.next-button').addEventListener('click', (event) => {
                event.preventDefault(); // Prevent default action
                changeSlide(1);
                // Reset auto-slide timer
                clearTimeout(autoSlideTimeout);
                autoSlideTimeout = setTimeout(autoSlide, 5000);
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Chi Tiết Phòng</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item"><a href="Room.aspx">Loại  Phòng</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Chi Tiết Phòng</li>
                    </ul>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfRoomId" runat="server" />
        <div class="site-content container">
            <div class="room-single row">
                <asp:Label ID="lblErrorMessage" runat="server" Visible="false" Text="" CssClass="success-message"></asp:Label>
                <asp:ListView ID="lstViewEmployeeDetails" runat="server">
                    <ItemTemplate>
                        <main class="site-main col-sm-12 col-md-12 flex-first">
                            <div class="room-wrapper">
                                <div class="room_gallery clearfix">
                                    <%--<div class=" camera_emboss" id="camera_wrap" style="text-align: center;">
                                        <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' class="slider-image " alt="" style="width: 100% !important; height: 469px !important;">
                                    </div>--%>
                                    <div class="slider-container">
                                        <div class="slider-wrapper">
                                            <img src="https://image.slidesdocs.com/responsive-images/background/modern-luxury-living-room-3d-illustration-of-an-interior-scene-with-a-mockup-and-decorated-wall-powerpoint-background_9be1ecf500__960_540.jpg" alt="Image 1" class="slider-image ">
                                            <img src="https://image.slidesdocs.com/responsive-images/background/a-plain-and-welcoming-space-3d-visualisation-of-a-white-room-with-wooden-floor-and-sofa-powerpoint-background_031d7e271c__960_540.jpg" alt="Image 2" class="slider-image">
                                            <img src="https://image.slidesdocs.com/responsive-images/background/sleek-3d-room-design-white-walls-organic-vase-and-warm-wood-flooring-powerpoint-background_7958c6a244__960_540.jpg" alt="Image 3" class="slider-image">
                                            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR0JvWyK-ZiRejps6y1e2OMAAUEB0k9n853kxYXkxgC_CYMrgAY4cP2H1rqOWnxAw1z7Y0&usqp=CAU" alt="Image 4" class="slider-image">
                                        </div>
                                        <button class="slider-button prev-button">&#10094;</button>
                                        <button class="slider-button next-button">&#10095;</button>
                                    </div>

                                </div>
                                <div class="title-share clearfix">
                                    <h2 class="title"><%# Eval("name") %></h2>
                                    <%--<a href="/Website/Cart.aspx?id=<%# Eval("id") %>" class="book-room">Book This Room</a>--%>
                                    <div class="social-share">
                                        <ul>
                                            <li><a class="link facebook" title="Facebook" href="https://www.facebook.com/ngocquang.nguyen.16100921" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-facebook"></i></a></li>
                                            <li><a class="link twitter" title="Twitter" href="#" text="TheTitleBlog" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-twitter"></i></a></li>
                                            <li><a class="link google" title="Google" href="https://www.fcbarcelona.com/en/" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-googleplus"></i></a>
                                        </ul>
                                    </div>
                                </div>
                                <div class="room_price">
                                    <span class="price_value price_min">$<%# Eval("price") %></span>
                                    <span class="unit">Đêm</span>
                                </div>
                                <div class="description">
                                    <p>
                                       <%# Eval("description") %>
                                    </p>
                                </div>
                                <div class="room_additinal">
                                    <h3 class="title style-01">TIỆN ÍCH VÀ DỊCH VỤ</h3>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <ul>
                                                <li><i class="fas fa-swimmer"></i>Hồ bơi ngoài trời</li>
                                                <li><i class="fas fa-wifi"></i>WiFi miễn phí</li>
                                                <li><i class="fas fa-plane"></i>Xe đưa đón sân bay</li>
                                            </ul>
                                        </div>
                                        <div class="col-sm-4">
                                            <ul>
                                                <li><i class="fas fa-umbrella-beach"></i>Giáp biển</li>
                                                <li><i class="fas fa-hand-holding-medical"></i>Trung tâm Spa & chăm sóc sức khoẻ</li>
                                                <li><i class="fas fa-smoking-ban"></i>Phòng không hút thuốc</li>
                                            </ul>
                                        </div>
                                        <div class="col-sm-4">
                                            <ul>
                                                <li><i class="fas fa-glass-cheers"></i>Quầy bar</li>
                                                <li><i class="fas fa-utensils"></i>Nhà hàng</li>
                                                <li><i class="fas fa-users"></i>Phòng gia đình</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </main>
                        <a href="/Website/Cart.aspx?id=<%# Eval("id") %>" class="btn btn-secondary cart mb-2" style="height: 57px; margin-left: 15px;">Thêm Vào Giỏ Hàng</a>
                    </ItemTemplate>
                </asp:ListView>

                <div id="secondary" class="widget-area col-sm-12 col-md-3 sticky-sidebar">
                    <div class="wd wd-book-room">
                        <a href="#" class="book-room bookingroom" style="border-radius: 4px;">Đặt Phòng Ngay</a>
                        <%--<asp:Button Text="success" OnClientClick="return showSuccessToast();" runat="server" CssClass="btn btn-outline-primary mt-2" />--%>
                        <div id="form-popup-room" class="form-popup-room">
                            <div class="popup-container">
                                <a href="#" class="close-popup"><i class="ion-android-close"></i></a>
                                <form id="hotel-popup-results" name="hb-search-single-room" class="hotel-popup-results">
                                    <div class="room-head">
                                        <h3 class="room-title">Khách Sạn Lionn</h3>
                                        <p class="description">Vui lòng nhập thông tin để hoàn thành việc đặt phòng này.</p>
                                    </div>
                                    <div class="search-room-popup">
                                        <ul class="form-table clearfix">
                                            <li class="form-field">
                                                <label>Nhập Số Lượng Người</label>
                                                <asp:TextBox ID="txtNumOfPeople" TextMode="Number" min="1" max="5" runat="server" class="add numpp w-100 bookings" placeholder="Số Lượng Người ...*"></asp:TextBox>
                                            </li>
                                            <li class="form-field">
                                                <label>Thời Gian Vào Phòng</label>
                                                <asp:TextBox ID="txtCheckIn" runat="server" class="check_in_date datebd bookings" TextMode="DateTimeLocal" placeholder="Arrival Date"></asp:TextBox>
                                            </li>
                                            <li class="form-field">
                                                <label>Thời Gian Trả Phòng</label>
                                                <asp:TextBox ID="txtCheckOut" runat="server" class="check_out_date datebd bookings" TextMode="DateTimeLocal" placeholder="Departure Date"></asp:TextBox>
                                            </li>
                                            <li class="form-field room-submit">
                                                <asp:Button Text="Đặt Phòng" OnClick="Datphong_Click" ID="btnSave" runat="server" CssClass="btn btn-outline-primary" OnClientClick="return validateDates();" />
                                            </li>
                                            <li class="form-field">
                                                <asp:Label ID="lblSuccess" runat="server" Visible="false" Text="" CssClass="success-message"></asp:Label>
                                            </li>
                                        </ul>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <%--<div class="wd wd-check-room">
                        Nguyễn Ngọc Quang
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
