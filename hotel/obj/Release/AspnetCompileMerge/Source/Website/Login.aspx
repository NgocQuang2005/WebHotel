<%@ Page Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="hotel.Website.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Đăng nhập</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Public+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/fonts/boxicons.css" />

    <!-- Core CSS -->
   <%-- <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/css/core.css" class="template-customizer-core-css" />
    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/css/theme-default.css" class="template-customizer-theme-css" />
    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/css/demo.css" />--%>

    <!-- Vendors CSS -->
    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/libs/perfect-scrollbar/perfect-scrollbar.css" />

    <!-- Page CSS -->
    <!-- Page -->
    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/css/pages/page-auth.css" />
    <!-- Helpers -->
    <script src="../App_Themes/Web_Pages/login/vendor/js/helpers.js"></script>
    <script src="../App_Themes/Web_Pages/login/js/config.js"></script>
    <style>
        .login {
            text-align: center;
            font-size: 23px;
            font-family: math;
        }

        a.createac {
            padding: 8px 132px !important;
            border: 1px solid #96A4C5;
            border-radius: 7px;
            color: black;
        }

            a.createac:hover {
                background-color: #96A4C5;
                color: #fff !important;
            }

        .nhomk {
            width: 100%;
            height: 100%;
        }

        .eyes {
            background-color: #fff !important;
            border-top: 1px solid #eee !important;
            border-right: 1px solid #eee !important;
            border-bottom: 1px solid #eee !important;
        }
        .pass {
            border-radius: 5px;
            border: 1px solid #eee !important;
        }

            .pass:hover {
                border: 1px solid #ced4da !important;
            }
            .input-group-text{
                border: none !important;
            }
            .form-control{
                 border: none ;
            }
            input[type="password"]{
                border: none !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h1 class="heading_primary">Đăng Nhập</h1>
                    <%--<ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item active">Danh Sách Sự Kiện</li>
                    </ul>--%>
                </div>
            </div>
        </div>
        <div class="container-xxl">
            <div class="authentication-wrapper authentication-basic container-p-y">
                <div class="authentication-inner">
                    <div class="card">
                        <div class="card-body">
                            <h2 class="login">Đăng Nhập</h2>
                            <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />
                            <div>
                                <label for="username" class="form-label ">Tên Đăng Nhập</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control pass" PlaceHolder="Tên đăng nhập" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="Yêu cầu tên đăng nhập" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
                            <div class=" form-password-toggle">
                                <div class="d-flex justify-content-between">
                                    <label class="form-label" for="password">Mật Khẩu</label>
                                    <a href="ForgotPassword.aspx">
                                        <small>Quên mật khẩu?</small>
                                    </a>
                                </div>
                                <div class="input-group input-group-merge pass">
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" PlaceHolder="Mật khẩu" autocomplete="new-password"></asp:TextBox>
                                    <span class="input-group-text cursor-pointer eyes"><i class="bx bx-hide"></i></span>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Yêu cầu nhập mật khẩu" CssClass="text-danger"></asp:RequiredFieldValidator>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input " />
                                    <label class="form-check-label" for="remember-me">Nhớ mật khẩu</label>
                                </div>
                            </div>
                            <div class="mb-3">
                                <asp:Button ID="btnSignIn" runat="server" CssClass="btn btn-primary d-grid w-100" Text="Đăng nhập" OnClick="btnSignIn_Click" />
                            </div>
                            <p class="text-center " style="padding: 8px 0px;">
                                <a href="Signup.aspx" class="createac">
                                    <span>Tạo Tài Khoản</span>
                                </a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <%--<script src="../App_Themes/Web_Pages/login/vendor/libs/jquery/jquery.js"></script>--%>
    <script src="../App_Themes/Web_Pages/login/vendor/libs/popper/popper.js"></script>
    <script src="../App_Themes/Web_Pages/login/vendor/js/bootstrap.js"></script>
    <script src="../App_Themes/Web_Pages/login/vendor/libs/perfect-scrollbar/perfect-scrollbar.js"></script>
    <script src="../App_Themes/Web_Pages/login/vendor/js/menu.js"></script>
    <script src="../App_Themes/Web_Pages/login/js/main.js"></script>
</asp:Content>

