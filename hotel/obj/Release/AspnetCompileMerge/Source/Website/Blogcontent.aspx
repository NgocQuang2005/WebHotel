<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Blogcontent.aspx.cs" Inherits="hotel.Website.Blogcontent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Blog Content</title>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h1 class="heading_primary">Nội Dung Bài Viết</h1>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item"><a href="Blog.aspx">Bài Viết</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Nội Dung Bài Viết</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="site-content container">
            <div class="row">
                <asp:ListView ID="lstViewEmployeeDetails" runat="server">
                    <ItemTemplate>
                        <main class="site-main col-sm-12 col-md-9 flex-first">
                            <div class="blog-single-content">
                                <article class="post clearfix">
                                    <div class="post-content">
                                        <div class="post-media">
                                            <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' alt="" style="width: 100%;">
                                        </div>
                                        <div class="post-summary">
                                            <h2 class="post-title"><%# Eval("title") %></h2>
                                            <ul class="post-meta">
                                                <li>by <a href="#"><%# Eval("name") %></a></li>
                                                <li><span class="separator"></span></li>
                                                <li><%# Eval("date") %></li>
                                                <li><span class="separator"></span></li>

                                            </ul>
                                            <div class="post-description">
                                                <blockquote>
                                                    <i class="fa fa-quote-left"></i>
                                                    <span>Chào Mừng Đến Với Khách Sạn Lionn.</span>
                                                </blockquote>
                                                <p><%# Eval("content") %></p>

                                            </div>
                                            <div class="meta_post">
                                                <div class="social-share">
                                                    <ul>
                                                        <li><a class="link facebook" title="Facebook" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-facebook"></i></a></li>
                                                        <li><a class="link twitter" title="Twitter" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-twitter"></i></a></li>
                                                        <li><a class="link pinterest" title="Pinterest" href="#" onclick="window.open(this.href, 'mywin','left=50,top=50,width=600,height=350,toolbar=0'); return false;"><i class="ion-social-pinterest"></i></a></li>
                                                        <li><a class="link google" title="Google" href="#" rel="nofollow" onclick="window.open(this.href,this.title,'width=600,height=600,top=200px,left=200px');  return false;" target="_blank"><i class="ion-social-googleplus"></i></a>
                                                    </ul>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </article>
                            </div>
                        </main>
                    </ItemTemplate>
                </asp:ListView>
                <aside id="secondary" class="widget-area col-sm-12 col-md-3 sticky-sidebar">
                    <div class="wd wd-categories">
                        <h3 class="wd-title">Thể  Loại</h3>
                    </div>
                    <div class="wd wd-image-box">
                        <div class="image-box">
                            <img src="../App_Themes/Web_Pages/images/gallery/masonry-5.jpg" alt="">
                        </div>
                    </div>
                </aside>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    
</asp:Content>
