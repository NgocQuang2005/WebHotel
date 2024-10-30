<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="EventContent.aspx.cs" Inherits="hotel.Website.EventContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Nội Dung Sự Kiện</title>
  
    <!-- Style -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h1 class="heading_primary">Sự Kiện Đơn</h1>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item"><a href="Event.aspx">Sự Kiện</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Sự Kiện Đơn</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="site-content container">
            <div class="row">
                <main class="site-main col-sm-12 col-md-9 flex-first">
                    <div class="event-single-content expired">
                        <div class="event clearfix">
                            <div class="wrapper">
                                <asp:ListView ID="ListViewEventContent" runat="server">
                                    <ItemTemplate>
                                        <div class="thumbnail ">
                                            <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt="">
                                        </div>
                                        <div class="content">
                                            <h2 class="title mt-2 mb-1"><%# Eval("title") %></h2>
                                            <div class="meta mb-3 ">
                                                <span class="time"><i class="fa fa-clock-o"></i><b><%# Eval("Date") %></b>  |</span>
                                                <span class="location"><i class="fa fa-map-marker"></i><b>Khách Sạn Lionn</b></span>
                                            </div>
                                            <div class="detail clearfix">
                                                <div class="">
                                                    <h4>Nội Dung Sự Kiện</h4>
                                                    <div>
                                                        <p><%# Eval("content") %></p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                                <div class="social-share">
                                    <ul>
                                        <li><a class="link facebook" title="Facebook" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-facebook"></i></a></li>
                                        <li><a class="link twitter" title="Twitter" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-twitter"></i></a></li>
                                        <li><a class="link google" title="Google" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-googleplus"></i></a>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </main>
                <aside id="secondary" class="widget-area col-sm-12 col-md-3 sticky-sidebar">
                    <div class="wd wd-book-event ">
                        <h3 class="wd-title">Các sự kiện khác</h3>
                        <asp:ListView ID="ListViewOther" runat="server">
                            <ItemTemplate>
                                <a href="Event.aspx">
                                    <div style="display: flex; justify-content: space-around; padding: 5px 0px; border-bottom: 1px solid #ebe3e3; margin: 0px 0px 5px 0px;">
                                        <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt="" style="width: 32%; border-radius: 7px;" />
                                        <p style="color: black;">
                                            <%# Eval("title") %>
                                            <label style="font-size: 12px; color: #aba3a3;">
                                                <%# Eval("date") %>
                                            </label>
                                        </p>
                                    </div>
                                </a>
                            </ItemTemplate>
                        </asp:ListView>
                        <p style="position: absolute; right: 16px; font-size: 12px; color: #717171;">
                            @NgocQuang
                        </p>
                    </div>
                </aside>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    
    <script>
        $(".thim-countdown .count-down").mbComingsoon({
            expiryDate: new Date($(".thim-countdown").data('date')),
            speed: 100
        });
    </script>
</asp:Content>
