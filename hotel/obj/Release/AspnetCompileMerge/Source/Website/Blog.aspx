<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Blog.aspx.cs" Inherits="hotel.Website.Blog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Blog</title>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Các Bài Viết Khách Sạn Lionn</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Bài Viết </li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="site-content container">
            <div class="row">
                <aside id="secondary" class="widget-area col-sm-12 col-md-3 sticky-sidebar">
                    <div class="wd wd-search">
                        <form role="search" method="get" action="#">
                            <input type="search" placeholder="Search …" value="" name="s">
                            <button type="submit"><i class="ion ion-ios-search"></i></button>
                        </form>
                    </div>
                    <div class="wd wd-categories">
                        <a href="Blog.aspx"><h3 class="wd-title">Thể Loại</h3></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="btnHotel" runat="server" OnClick="FilterByKeyword" CommandArgument="HOTEL">Khách Sạn</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="btnActivities" runat="server" OnClick="FilterByKeyword" CommandArgument="ACTIVITIES">Các Hoạt Động</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="btnRestaurant" runat="server" OnClick="FilterByKeyword" CommandArgument="RESTAURANT">Nhà Hàng</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="btnTipTravel" runat="server" OnClick="FilterByKeyword" CommandArgument="TIP TRAVEL">TIP TRAVEL</asp:LinkButton></li>
                        </ul>
                    </div>
                    <div class="wd wd-image-box">
                        <div class="image-box">
                            <img src="../App_Themes/Web_Pages/images/gallery/masonry-5.jpg" alt="">
                        </div>
                    </div>
                </aside>

                <main class="site-main col-sm-12 col-md-9 flex-first">
                    <div class="blog-content layout-grid">
                        <div class="row" id="blogContainer">
                            <asp:ListView ID="lstViewEmployeeDetails" runat="server">
                                <ItemTemplate>
                                    <article class="post col-sm-6 clearfix" data-page="1">
                                        <div class="post-content">
                                            <div class="post-media">
                                                <a href="<%# "Blogcontent.aspx?id=" + Eval("id").ToString() %>">
                                                    <img src="<%# ResolveUrl(Eval("URL").ToString()) %>" alt="" style="height: 237px; width: 100%;"></a>
                                            </div>
                                            <div class="post-summary">
                                                <h2 class="post-title">
                                                    <a href="<%# "Blogcontent.aspx?id=" + Eval("id").ToString() %>"><%# Eval("title") %></a>
                                                </h2>
                                                <ul class="post-meta">
                                                    <li>by <a href="<%# "Blogcontent.aspx?id=" + Eval("id").ToString() %>"><%# Eval("name") %></a></li>
                                                    <li><span class="separator"></span></li>
                                                    <li><%# Eval("date") %></li>
                                                    <li><span class="separator"></span></li>
                                                </ul>
                                                <p class="post-description" lang="zxx"><%# TruncateContent(Eval("content").ToString(), 30) %></p>
                                                <a href="<%# "Blogcontent.aspx?id=" + Eval("id").ToString() %>" class="btn-icon">Read more</a>
                                            </div>
                                        </div>
                                    </article>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>

                        <ul class="loop-pagination" id="pageNavPosition">
                            <li><a href="#" id="prevPage"><i class="fa fa-angle-left"></i></a></li>
                            <li><a href="#" class="paginationLink" data-page="1">1</a></li>
                            <li><a href="#" class="paginationLink" data-page="2">2</a></li>
                            <%--<li><a href="#" class="paginationLink" data-page="3">3</a></li>--%>
                            <li><a href="#" id="nextPage"><i class="fa fa-angle-right"></i></a></li>
                        </ul>
                    </div>
                </main>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const roomsPerPage = 4;  // Số lượng phòng hiển thị trên mỗi trang
            const blogContainer = document.getElementById('blogContainer');
            const rooms = blogContainer.getElementsByClassName('post');
            const paginationLinks = document.querySelectorAll('.paginationLink');
            let currentPage = 1;

            function showPage(page) {
                currentPage = page;
                const start = (page - 1) * roomsPerPage;
                const end = start + roomsPerPage;

                Array.from(rooms).forEach((room, index) => {
                    room.style.display = (index >= start && index < end) ? 'block' : 'none';
                });

                paginationLinks.forEach(link => {
                    link.parentElement.classList.toggle('active', parseInt(link.getAttribute('data-page')) === page);
                });
            }

            paginationLinks.forEach(link => {
                link.addEventListener('click', function (e) {
                    e.preventDefault();
                    showPage(parseInt(this.getAttribute('data-page')));
                });
            });

            document.getElementById('prevPage').addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage > 1) {
                    showPage(currentPage - 1);
                }
            });

            document.getElementById('nextPage').addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage < Math.ceil(rooms.length / roomsPerPage)) {
                    showPage(currentPage + 1);
                }
            });

            showPage(1);  // Hiển thị trang đầu tiên ban đầu
        });
    </script>
    
</asp:Content>
