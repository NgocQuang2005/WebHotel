<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="hotel.Website.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Liên Hệ</title>
    
    <style>
        .j97 {
            height: 50px;
            margin-bottom: 20px;
        }

        .bdlayout {
            border-color: #d9dee3;
            border-radius: 0.5rem;
            box-shadow: 0px 0px 10px 7px rgba(67, 89, 113, 0.12);
            background-color: #fff;
        }

        /*.diric {
            display: flex;
        }

        @media only screen and (max-width: 1200px) {
            .diric {
                display: block;
                padding-top: 15px;
            }

            

            .ggmap {
                padding-top: 15px;
            }
        }*/

        .responsive-iframe {
            width: 100%;
            max-width: 100%;
        }

        .dismap {
            padding-left: 40px;
            padding-right: 40px;
        }
        .messeage{
            border-radius: 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content" style="background-color: #f5f5f9;" class="pb-5">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h2 class="heading_primary">Liên hệ Khách Sạn Lionn</h2>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Liên Hệ </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="site-content no-padding ">
            <div class="page-content">
                <div class="container">
                    <div class="empty-space"></div>
                    <div class="row tm-flex">
                        <div class="col-sm-12 bdlayout p-4"  style="background-color: #f7f4f2 !important;">
                            <div class="sc-contact-form bdlayout mb-4">
                                <div class="sc-heading mb-3">
                                <p class="first-title">GỬI 1</p>
                                <h3 class="second-title">TIN NHẮN</h3>
                                <p class="description">
                                    Bạn có điều gì muốn nói với chúng tôi không?
                                    <br>
                                    Xin đừng ngần ngại liên hệ với chúng tôi thông qua mẫu liên hệ của chúng tôi.
                                </p>
                            </div>
                                <form action="#" id="ajaxform" method="post">
                                    <asp:TextBox ID="txtMessage" runat="server"  CssClass="messeage" TextMode="MultiLine" Rows="5" placeholder="Lời Nhắn Của Bạn*" />
                                    <%--button--%>
                                    <asp:Button ID="btnSubmit" runat="server" Text="Gửi" OnClick="btnSubmit_Click" class="submit btn btn-outline-success mt-4 " />
                                    <asp:Button ID="btnCancel" runat="server" Text="Hủy" OnClick="btnCancel_Click" class="submit btn btn-outline-danger mt-4 " />
                                    <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red" />
                                </form>
                            </div>
                            <asp:ListView ID="ListViewContact" runat="server">
                                <ItemTemplate>
                                    <div class="dismap col-sm-12 bdlayout py-4 diric">
                                        <div class="px-3 pb-3  ">
                                            <div class="sc-heading">
                                                <p class="first-title">LIÊN LẠC</p>
                                                <p class="description">
                                                    <a href="#"><i class="fas fa-map-marker-alt"></i>  <%# Eval("address") %></a>
                                                </p>
                                            </div>
                                            <p class="phone" style="margin: 0px"><i class="fas fa-phone-alt"></i>  <a href="#"><%# Eval("phone") %></a></p>
                                            <p class="email" style="margin: 0px"><i class="fas fa-envelope"></i>  <a href="#"><%# Eval("email") %></a></p>
                                            <ul class="sc-social-link style-03">
                                                <li>
                                                    <a target="_blank" class="face" href="#" title="Facebook">
                                                        <i class="fa fa-facebook"></i>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a target="_blank" class="twitter" href="#" title="Twitter">
                                                        <i class="fa fa-twitter"></i>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a target="_blank" class="skype" href="#" title="Skype">
                                                        <i class="fa fa-skype"></i>
                                                    </a>
                                                </li>
                                                <li>
                                                    <a class="instagram" href="#" title="Instagram">
                                                        <i class="fa fa-instagram"></i>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="px-3 responsive-iframe ">
                                            <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d8251.618885424126!2d108.2264635884883!3d16.07649266332212!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x314219bd48cb3729%3A0x64c48e8bf0217b03!2sCoffee%E2%80%99s%20House!5e1!3m2!1svi!2s!4v1718956664254!5m2!1svi!2s" width="100%" height="300" style="border: 0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
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
