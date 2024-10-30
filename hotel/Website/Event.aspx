<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Event.aspx.cs" Inherits="hotel.Website.Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Sự kiện</title>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Sự Kiện</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Danh Sách Sự Kiện</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="site-content container">
            <div class="events-content">
                <div class="sc-events list-style">
                    <nav class="filter-events">
                        <div class="nav nav-tabs" id="nav-tab" role="tablist">
                            <a class="nav-item nav-link active" id="happening-tab" data-toggle="tab" href="#happening" role="tab" aria-selected="true">Đang Diễn Ra</a>
                            <a class="nav-item nav-link" id="nav-upcoming-tab" data-toggle="tab" href="#upcoming" role="tab" aria-selected="false">Sắp Tới</a>
                        </div>
                    </nav>

                    <div class="tab-content" id="nav-tabContent">
                        <div class="tab-pane fade show active" id="happening" role="tabpanel">
                            <asp:ListView ID="ListViewHappening" runat="server">
                                <ItemTemplate>

                                    <div class="event">
                                        <div class="row tm-flex">
                                            <div class="col-lg-7 col-md-6">
                                                <div class="event-content pl-1 pt-3">
                                                    <h3 class="title"><a href="<%# "EventContent.aspx?id=" + Eval("id").ToString() %>"><%# Eval("title") %></a></h3>
                                                    <div class="meta">
                                                        <span class="time"><i class="fa fa-clock-o"></i><%# Eval("date") %></span>
                                                        <span class="location"><i class="fa fa-map-marker"></i>Khách Sạn Lionn</span>
                                                    </div>
                                                    <div class="event-desc"><%# TruncateContent(Eval("content").ToString(), 30) %> </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-5 col-md-6">
                                                <div class="thumbnail">
                                                    <a href="<%# "EventContent.aspx?id=" + Eval("id").ToString() %>" >
                                                        <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt=""></a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <div class="tab-pane fade" id="upcoming" role="tabpanel" aria-labelledby="nav-upcoming-tab">
                            <asp:ListView ID="ListViewUpcoming" runat="server">
                                <ItemTemplate>

                                    <div class="event">
                                        <div class="row tm-flex">
                                            <div class="col-lg-7 col-md-6">
                                                <div class="event-content pl-5 pt-3">
                                                    <h3 class="title"><a href="<%# "EventContent.aspx?id=" + Eval("id").ToString() %>"><%# Eval("title") %></a></h3>
                                                    <div class="meta">
                                                        <span class="time"><i class="fa fa-clock-o"></i><%# Eval("date") %></span>
                                                        <span class="location"><i class="fa fa-map-marker"></i>Khách Sạn Lionn</span>
                                                    </div>
                                                    <div class="event-desc"><%# TruncateContent(Eval("content").ToString(), 30) %> </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-5 col-md-6">
                                                <div class="thumbnail">
                                                    <a href="<%# "EventContent.aspx?id=" + Eval("id").ToString() %>" >
                                                        <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt=""></a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
