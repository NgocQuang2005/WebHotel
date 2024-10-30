<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="hotel.Website.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang Chủ</title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Web_Pages/css/libs/revolution/settings.css">
    <%-- <style>
        .btn {
            padding: 6px 12px;
            border: 1px solid #0d6efd;
            color: #0d6efd;
            background-color: #fff;
            border-radius: 0.375rem;
        }

            .btn:hover {
                background-color: #0d6efd;
                color: #fff;
            }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content" class="main-content">
        <div id="home-main-content" class="home-main-content home-1">
            <div id="rev_slider_2_1_wrapper" class="rev_slider_wrapper fullscreen-container no-mask-bg" data-alias="home-3"
                data-source="gallery"
                style="padding: 0px; background-image: url('../App_Themes/Web_Pages/images/slides/h3-slider3.jpg'); background-repeat: no-repeat; background-size: cover; background-position: center center;">
                <!-- START REVOLUTION SLIDER 5.4.7.4 fullscreen mode -->
                <div id="rev_slider_2_1" class="rev_slider fullscreenbanner" style="display: none;" data-version="5.4.7.4">
                    <ul>
                        <!-- SLIDE  -->
                        <li data-index="rs-6" data-transition="fade" data-slotamount="default" data-hideafterloop="0"
                            data-hideslideonmobile="off" data-easein="default" data-easeout="default" data-masterspeed="300"
                            data-thumb="../App_Themes/Web_Pages/images/slides/h3-slider3-100x50.jpg" data-rotate="0" data-saveperformance="off"
                            data-title="Slide" data-param1="" data-param2="" data-param3="" data-param4="" data-param5=""
                            data-param6="" data-param7="" data-param8="" data-param9="" data-param10="" data-description="">
                            <!-- MAIN IMAGE -->
                            <img src="../App_Themes/Web_Pages/images/slides/h3-slider3.jpg" alt="" data-bgposition="center center" data-bgfit="cover"
                                data-bgrepeat="no-repeat" class="rev-slidebg" data-no-retina="">
                            <!-- LAYERS -->
                            <!-- LAYER NR. 7 -->
                            <h3 class="tp-caption   tp-resizeme"
                                id="slide-6-layer-1"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['-60','-60','-60','-60']"
                                data-fontsize="['64','64','64','40']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"x:{-250,250};y:{-150,150};rX:{-90,90};rY:{-90,90};rZ:{-360,360};sX:0;sY:0;opacity:0;","speed":1500,"to":"o:1;","delay":300,"ease":"Power3.easeInOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"Power2.easeIn"}]'
                                data-textalign="['center','center','center','center']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 11; white-space: nowrap; font-size: 64px; font-weight: 700; color: rgba(255,255,255,1); text-transform: uppercase;">MỘT KHÁCH SẠN  5 SAO
                            </h3>

                            <!-- LAYER NR. 8 -->
                            <p class="tp-caption   tp-resizeme"
                                id="slide-6-layer-2"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['5','5','5','5']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:bottom;rX:-20deg;rY:-20deg;rZ:0deg;","speed":1500,"to":"o:1;","delay":900,"ease":"Power3.easeOut"},{"delay":"wait","speed":300,"to":"y:[-100%];","mask":"x:inherit;y:inherit;s:inherit;e:inherit;","ease":"nothing"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 12; white-space: nowrap; font-size: 20px; line-height: 22px; font-weight: 400; color: rgba(255,255,255,1);">
                                Và chúng tôi muốn giữ nó theo cách đó!
                            </p>

                            <!-- LAYER NR. 9 -->
                            <div class="tp-caption   tp-resizeme  thim-link-slider"
                                id="slide-6-layer-3"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['60','60','60','60']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:bottom;rX:-20deg;rY:-20deg;rZ:0deg;","speed":1500,"to":"o:1;","delay":900,"ease":"Power3.easeOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"nothing"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 13; white-space: nowrap; font-size: 15px; font-weight: 700; color: rgba(255,255,255,1); text-transform: uppercase;">
                                <a href="#">LIÊN HỆ</a>
                            </div>
                        </li>
                        <!-- SLIDE  -->
                        <li data-index="rs-5" data-transition="fade" data-slotamount="default" data-hideafterloop="0"
                            data-hideslideonmobile="off" data-easein="default" data-easeout="default" data-masterspeed="300"
                            data-thumb="../App_Themes/Web_Pages/images/slides/h3-slider2-100x50.jpg" data-rotate="0" data-saveperformance="off"
                            data-title="Slide" data-param1="" data-param2="" data-param3="" data-param4="" data-param5=""
                            data-param6="" data-param7="" data-param8="" data-param9="" data-param10="" data-description="">
                            <!-- MAIN IMAGE -->
                            <img src="../App_Themes/Web_Pages/images/slides/h3-slider2.jpg" alt="" data-bgposition="center center" data-bgfit="cover"
                                data-bgrepeat="no-repeat" class="rev-slidebg" data-no-retina="">
                            <!-- LAYERS -->
                            <!-- LAYER NR. 4 -->
                            <h3 class="tp-caption   tp-resizeme"
                                id="slide-5-layer-1"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['-60','-60','-60','-60']"
                                data-fontsize="['64','64','50','32']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:[100%];z:0;rZ:-35deg;sX:1;sY:1;skX:0;skY:0;","mask":"x:0px;y:0px;s:inherit;e:inherit;","speed":2000,"to":"o:1;","delay":300,"split":"chars","splitdelay":0.05,"ease":"Power4.easeInOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"Power2.easeIn"}]'
                                data-textalign="['center','center','center','center']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 8; white-space: nowrap; font-size: 64px; font-weight: 700; color: rgba(255,255,255,1); text-transform: uppercase;">ĐẶT SỚM TIẾT KIỆM NHIỀU HƠN
                            </h3>

                            <!-- LAYER NR. 5 -->
                            <p class="tp-caption   tp-resizeme"
                                id="slide-5-layer-2"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['5','5','5','5']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"x:[105%];z:0;rX:45deg;rY:0deg;rZ:90deg;sX:1;sY:1;skX:0;skY:0;","mask":"x:0px;y:0px;s:inherit;e:inherit;","speed":2000,"to":"o:1;","delay":500,"split":"chars","splitdelay":0.05,"ease":"Power4.easeInOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"Power2.easeIn"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 9; white-space: nowrap; font-size: 20px; line-height: 22px; font-weight: 400; color: rgba(255,255,255,1);">
                                Kiểm tra tình trạng phòng trống và đặt phòng
                            </p>

                            <!-- LAYER NR. 6 -->
                            <div class="tp-caption   tp-resizeme  thim-link-slider"
                                id="slide-5-layer-3"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['60','60','60','60']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"z:0;rX:0;rY:0;rZ:0;sX:0.9;sY:0.9;skX:0;skY:0;opacity:0;","speed":1500,"to":"o:1;","delay":1300,"ease":"Power3.easeInOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"Power2.easeIn"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 10; white-space: nowrap; font-size: 15px; font-weight: 700; color: rgba(255,255,255,1); text-transform: uppercase;">
                                <a href="#">KHÁM PHÁ</a>
                            </div>
                        </li>
                        <!-- SLIDE  -->
                        <li data-index="rs-4" data-transition="fade" data-slotamount="default" data-hideafterloop="0"
                            data-hideslideonmobile="off" data-easein="default" data-easeout="default" data-masterspeed="300"
                            data-thumb="../App_Themes/Web_Pages/images/slides/h3-slider1-100x50.jpg" data-rotate="0" data-saveperformance="off"
                            data-title="Slide" data-param1="" data-param2="" data-param3="" data-param4="" data-param5=""
                            data-param6="" data-param7="" data-param8="" data-param9="" data-param10="" data-description="">
                            <!-- MAIN IMAGE -->
                            <img src="../App_Themes/Web_Pages/images/slides/h3-slider1.jpg" alt="" data-bgposition="center center" data-bgfit="cover"
                                data-bgrepeat="no-repeat" class="rev-slidebg" data-no-retina="">
                            <!-- LAYERS -->
                            <!-- LAYER NR. 1 -->
                            <h3 class="tp-caption   tp-resizeme"
                                id="slide-4-layer-1"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['-60','-60','-60','-60']"
                                data-fontsize="['64','64','64','40']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:50px;opacity:0;","speed":1500,"to":"o:1;","delay":900,"ease":"Power3.easeInOut"},{"delay":"wait","speed":100,"to":"y:-50px;opacity:0;","ease":"nothing"}]'
                                data-textalign="['center','center','center','center']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 5; white-space: nowrap; font-size: 64px; font-weight: 700; color: rgba(255,255,255,1);">LIONN
                            </h3>

                            <!-- LAYER NR. 2 -->
                            <p class="tp-caption   tp-resizeme"
                                id="slide-4-layer-2"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['5','5','5','5']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:bottom;rX:-20deg;rY:-20deg;rZ:0deg;","speed":1500,"to":"o:1;","delay":900,"ease":"Power3.easeOut"},{"delay":"wait","speed":300,"to":"y:[-100%];","mask":"x:inherit;y:inherit;s:inherit;e:inherit;","ease":"nothing"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 6; white-space: nowrap; font-size: 20px; line-height: 22px; font-weight: 400; color: rgba(255,255,255,1);">
                                Khách sạn Lionn từ €50 mỗi đêm
                            </p>

                            <!-- LAYER NR. 3 -->
                            <div class="tp-caption   tp-resizeme  thim-link-slider"
                                id="slide-4-layer-3"
                                data-x="['center','center','center','center']" data-hoffset="['0','0','0','0']"
                                data-y="['middle','middle','middle','middle']" data-voffset="['60','60','60','60']"
                                data-width="none"
                                data-height="none"
                                data-whitespace="nowrap"
                                data-type="text"
                                data-responsive_offset="on"
                                data-frames='[{"from":"y:bottom;rX:-20deg;rY:-20deg;rZ:0deg;","speed":1500,"to":"o:1;","delay":900,"ease":"Power3.easeOut"},{"delay":"wait","speed":100,"to":"opacity:0;","ease":"nothing"}]'
                                data-textalign="['left','left','left','left']"
                                data-paddingtop="[0,0,0,0]"
                                data-paddingright="[0,0,0,0]"
                                data-paddingbottom="[0,0,0,0]"
                                data-paddingleft="[0,0,0,0]"
                                style="z-index: 7; white-space: nowrap; font-size: 15px; font-weight: 700; color: rgba(255,255,255,1); text-transform: uppercase;">
                                <a href="#">PHÁT HIỆN</a>
                            </div>
                        </li>
                    </ul>
                    <div class="tp-bannertimer tp-bottom" style="visibility: hidden !important;"></div>
                </div>
            </div>
            <!-- END REVOLUTION SLIDER -->
            <div class="empty-space"></div>
            <div class="container">
                <asp:ListView ID="lstViewEmployeeDetails" runat="server">
                    <ItemTemplate>
                        <div class="sc-heading">
                            <p class="first-title">WELCOME TO</p>
                            <h3 class="second-title"><%# Eval("title") %></h3>
                        </div>
                        <div class="sc-info about-info row">
                            <div class="col-sm-6">
                                <p><%# Eval("content") %></p>
                            </div>
                            <div class="col-sm-6">
                                <div class="sc-about-slides row">
                                    <ul class="slides owl-theme owl-carousel">
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-1.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-8.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-3.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-4.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-5.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-6.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-7.jpg" alt=""></li>
                                        <li>
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-2.jpg" alt=""></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>

            <div class="empty-space"></div>
            <div class="sc-categories-link style-02">
                <div class="container">
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="item">
                                <img src="../App_Themes/Web_Pages/images/home/h2-img1.jpg" alt="">
                                <a href="#" class="img-link"></a>
                                <div class="content-overlay">
                                    <h4 class="title"><a href="Room.aspx">Rooms</a></h4>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="item">
                                <img src="../App_Themes/Web_Pages/images/home/h2-img2.jpg" alt="">
                                <a href="#" class="img-link"></a>
                                <div class="content-overlay">
                                    <h4 class="title"><a href="#">Restaurant</a></h4>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="item">
                                <img src="../App_Themes/Web_Pages/images/home/h2-img3.jpg" alt="">
                                <a href="#" class="img-link"></a>
                                <div class="content-overlay">
                                    <h4 class="title"><a href="#">Spa</a></h4>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="item">
                                <img src="../App_Themes/Web_Pages/images/home/h2-img4.jpg" alt="">
                                <a href="#" class="img-link"></a>
                                <div class="content-overlay">
                                    <h4 class="title"><a href="Blog.aspx">Activities</a></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="empty-space"></div>
            <div class="sc-quote style-02">
                <div class="sc-content-overlay">
                    <div class="container">
                        <div class="content">
                            <h3 class="title">
                                <span>
                                    <b>Welcome </b>guests to
                                <br>
                                    our hotel.
                                </span>
                            </h3>
                        </div>
                    </div>
                </div>
            </div>

            <div class="empty-space"></div>
            <div class="sc-rooms style-02">
                <div class="container">
                    <div class="sc-heading style-03">
                        <p class="first-title">EXPLORE</p>
                        <h3 class="second-title">OUR ROOMS</h3>
                    </div>
                    <div class="rooms-content layout-grid style-02">
                        <div class="row">
                            <div class="room col-sm-4 clearfix">
                                <div class="room-item">
                                    <div class="room-media">
                                        <a href="#">
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-4.jpg" alt=""></a>
                                    </div>
                                </div>
                            </div>
                            <div class="room col-sm-4 clearfix">
                                <div class="room-item">
                                    <div class="room-media">
                                        <a href="#">
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-11.jpg" alt=""></a>
                                    </div>

                                </div>
                            </div>
                            <div class="room col-sm-4 clearfix">
                                <div class="room-item">
                                    <div class="room-media">
                                        <a href="#">
                                            <img src="../App_Themes/Web_Pages/images/gallery/img-6.jpg" alt=""></a>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="empty-space"></div>
            <div class="h2-testimonials">
                <div class="sc-content-overlay">
                    <div class="container">
                        <div class="sc-testimonials style-02 pull-right">
                            <div class="sc-heading">
                                <p class="first-title">KHÁCH HÀNG REVIEWS</p>
                            </div>
                            <div class="testimonial-slider2 owl-carousel owl-theme">
                                <asp:ListView ID="listviewreviews" runat="server">
                                    <ItemTemplate>
                                        <div class="item">
                                            <div class="content">
                                                "<%# Eval("content") %> "
                                            </div>
                                            <p class="review"><span class="rating-star"></span></p>
                                            <p class="name"><%# Eval("title") %></p>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="empty-space"></div>
            <div class="container blog-event">
                <div class="row">
                    <div class="col-sm-6 col-md-8 item-first">
                        <div class="sc-heading style-03">
                            <p class="first-title">FROM</p>
                            <h3 class="second-title">OUR BLOG</h3>
                        </div>
                        <div class="sc-post style-02">
                            <div class="row">
                                <asp:ListView ID="ListViewBlog" runat="server">
                                    <ItemTemplate>
                                        <div class="post col-sm-6">
                                            <div class="post-content">
                                                <div class="post-media">
                                                    <a href="Blog.aspx">
                                                        <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt="" style="width: 100%; height: 208px;"></a>
                                                </div>
                                                <div class="post-summary">
                                                    <div class="meta"><span class="date"><%# Eval("date") %></span></div>
                                                    <h4 class="title"><a href="Blog.aspx"><%# Eval("title") %></a></h4>
                                                    <div class="readmore">
                                                        <a href="Blog.aspx" class="btn-icon">Read More</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4 item-second">
                        <div class="sc-heading style-03">
                            <p class="first-title">NEXT</p>
                            <h3 class="second-title">EVENTS</h3>
                        </div>
                        <div class="sc-list-event style-01">
                            <div class="list">
                                <asp:ListView ID="ListViewOther" runat="server">
                                    <ItemTemplate>
                                        <div class="event clearfix">
                                            <div style="display: flex; justify-content: space-around; padding: 5px 0px; margin: 0px 0px 5px 0px;">
                                                <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt="" style="width: 42%; border-radius: 7px;" />
                                                <h5 style="color: black; text-align: center;">
                                                    <%# Eval("title") %>
                                                    <label style="font-size: 14px; color: #aba3a3;">
                                                        <%# Eval("date") %>
                                                    </label>
                                                </h5>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <a href="Event.aspx" class="btn-icon">Read More</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="empty-space"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <%-- <script src="../App_Themes/Web_Pages/js/libs/jquery-1.12.4.min.js"></script>
    <!-- jQuery 1-->
    <script src="../App_Themes/Web_Pages/js/libs/bootstrap.min.js"></script>
    <!-- Bootstrap 1-->
    <script src="../App_Themes/Web_Pages/js/libs/smoothscroll.min.js"></script>
    <!-- smoothscroll 1-->
    <script src="../App_Themes/Web_Pages/js/libs/owl.carousel.min.js"></script>
    <!-- Owl Carousel 1-->
    <script src="../App_Themes/Web_Pages/js/libs/jquery.magnific-popup.min.js"></script>
    <!-- Magnific Popup 1-->
    <script src="../App_Themes/Web_Pages/js/libs/theia-sticky-sidebar.min.js"></script>
    <!-- Sticky sidebar 1-->
    <script src="../App_Themes/Web_Pages/js/libs/stellar.min.js"></script>
    <!-- counter 0-->
    <script src="../App_Themes/Web_Pages/js/libs/counter-box.min.js"></script>
    <!-- counter 1-->
    <script src="../App_Themes/Web_Pages/js/libs/jquery.thim-content-slider.min.js"></script>
    <!-- Slider -->
    <script src="../App_Themes/Web_Pages/js/libs/moment.min.js"></script>
    <!-- moment -->
    <script src="../App_Themes/Web_Pages/js/libs/jquery-ui.min.js"></script>
    <!-- ui -->
    <script src="../App_Themes/Web_Pages/js/libs/daterangepicker.min.js"></script>
    <!-- date -->
    <script src="../App_Themes/Web_Pages/js/libs/daterangepicker.min-date.min.js"></script>
    <!-- date2 -->
    <script src="js/theme-customs.js"></script>--%>
    <!-- Theme Custom -->
    <!-- REVOLUTION JS FILES -->
    <script src="../App_Themes/Web_Pages/js/libs/revolution/jquery.themepunch.tools.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/jquery.themepunch.revolution.min.js"></script>

    <!-- SLIDER REVOLUTION 5.0 EXTENSIONS  (Load Extensions only on Local File Systems !  The following part can be removed on Server for On Demand Loading) -->
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.actions.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.carousel.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.kenburn.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.layeranimation.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.migration.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.navigation.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.parallax.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.slideanims.min.js"></script>
    <script src="../App_Themes/Web_Pages/js/libs/revolution/extensions/revolution.extension.video.min.js"></script>
    <script>
        /* Start OF WRAPPING FUNCTION */
        function setREVStartSize(e) {
            try {
                e.c = jQuery(e.c);
                var i = jQuery(window).width(), t = 9999, r = 0, n = 0, l = 0, f = 0, s = 0, h = 0;
                if (e.responsiveLevels && (jQuery.each(e.responsiveLevels, function (e, f) {
                    f > i && (t = r = f, l = e), i > f && f > r && (r = f, n = e)
                }), t > r && (l = n)), f = e.gridheight[l] || e.gridheight[0] || e.gridheight, s = e.gridwidth[l] || e.gridwidth[0] || e.gridwidth, h = i / s, h = h > 1 ? 1 : h, f = Math.round(h * f), "fullscreen" == e.sliderLayout) {
                    var u = (e.c.width(), jQuery(window).height());
                    if (void 0 != e.fullScreenOffsetContainer) {
                        var c = e.fullScreenOffsetContainer.split(",");
                        if (c) jQuery.each(c, function (e, i) {
                            u = jQuery(i).length > 0 ? u - jQuery(i).outerHeight(!0) : u
                        }), e.fullScreenOffset.split("%").length > 1 && void 0 != e.fullScreenOffset && e.fullScreenOffset.length > 0 ? u -= jQuery(window).height() * parseInt(e.fullScreenOffset, 0) / 100 : void 0 != e.fullScreenOffset && e.fullScreenOffset.length > 0 && (u -= parseInt(e.fullScreenOffset, 0))
                    }
                    f = u
                } else void 0 != e.minHeight && f < e.minHeight && (f = e.minHeight);
                e.c.closest(".rev_slider_wrapper").css({ height: f })
            } catch (d) {
                console.log("Failure at Presize of Slider:" + d)
            }
        };

        var revapi2,
            tpj;
        (function () {
            if (!/loaded|interactive|complete/.test(document.readyState)) document.addEventListener("DOMContentLoaded", onLoad); else onLoad();

            function onLoad() {
                if (tpj === undefined) {
                    tpj = jQuery;
                    if ("off" == "on") tpj.noConflict();
                }
                if (tpj("#rev_slider_2_1").revolution == undefined) {
                    revslider_showDoubleJqueryError("#rev_slider_2_1");
                } else {
                    revapi2 = tpj("#rev_slider_2_1").show().revolution({
                        sliderType: "standard",
                        sliderLayout: "fullscreen",
                        dottedOverlay: "none",
                        delay: 9000,
                        navigation: {
                            keyboardNavigation: "off",
                            keyboard_direction: "horizontal",
                            mouseScrollNavigation: "off",
                            mouseScrollReverse: "default",
                            onHoverStop: "off",
                            arrows: {
                                style: "zeus",
                                enable: true,
                                hide_onmobile: false,
                                hide_onleave: false,
                                tmp: '<div class="tp-title-wrap">  	<div class="tp-arr-imgholder"></div> </div>',
                                left: {
                                    h_align: "left",
                                    v_align: "center",
                                    h_offset: 30,
                                    v_offset: 0
                                },
                                right: {
                                    h_align: "right",
                                    v_align: "center",
                                    h_offset: 20,
                                    v_offset: 0
                                }
                            }
                            ,
                            bullets: {
                                enable: true,
                                hide_onmobile: false,
                                style: "hermes",
                                hide_onleave: false,
                                direction: "horizontal",
                                h_align: "center",
                                v_align: "bottom",
                                h_offset: 0,
                                v_offset: 60,
                                space: 25,
                                tmp: ''
                            }
                        },
                        viewPort: {
                            enable: true,
                            outof: "wait",
                            visible_area: "80%",
                            presize: false
                        },
                        responsiveLevels: [1240, 1024, 778, 480],
                        visibilityLevels: [1240, 1024, 778, 480],
                        gridwidth: [1240, 1024, 778, 480],
                        gridheight: [600, 600, 500, 400],
                        lazyType: "none",
                        shadow: 0,
                        spinner: "off",
                        stopLoop: "off",
                        stopAfterLoops: -1,
                        stopAtSlide: -1,
                        shuffle: "off",
                        autoHeight: "off",
                        fullScreenAutoWidth: "on",
                        fullScreenAlignForce: "off",
                        fullScreenOffsetContainer: "",
                        fullScreenOffset: "",
                        disableProgressBar: "on",
                        hideThumbsOnMobile: "off",
                        hideSliderAtLimit: 0,
                        hideCaptionAtLimit: 0,
                        hideAllCaptionAtLilmit: 0,
                        debugMode: false,
                        fallbacks: {
                            simplifyAll: "off",
                            nextSlideOnWindowFocus: "off",
                            disableFocusListener: false,
                        }
                    });
                }
                ;
                /* END OF revapi call */

            };
            /* END OF ON LOAD FUNCTION */
        }());
        /* End OF WRAPPING FUNCTION */

        var d = new Date();
        document.getElementById("day").setAttribute('value', d.getDate());
        document.getElementById("month").setAttribute('value', format_month());
        document.getElementById("multidate2").setAttribute('value', format_full());

        document.getElementById("day2").setAttribute('value', d.getDate() + 1);
        document.getElementById("month2").setAttribute('value', format_month());

        function format_full() {
            var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'September', 'November', 'December'];
            return months[d.getMonth()] + ' ' + d.getDate() + ', ' + d.getFullYear();
        }
        function format_month() {
            var months = ['Jan', 'Feb', 'March', 'April', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            return months[d.getMonth()];
        }

    </script>
</asp:Content>
