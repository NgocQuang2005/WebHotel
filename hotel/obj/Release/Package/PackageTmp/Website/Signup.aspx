<%@ Page Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="hotel.Website.Signup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="icon" type="image/x-icon" href="../App_Themes/Web_Pages/login/img/favicon/favicon.ico" />

    <!-- Fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link href="https://fonts.googleapis.com/css2?family=Public+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/fonts/boxicons.css" />

    <%-- <!-- Core CSS -->
    <link rel="stylesheet" href="../App_Themes/Web_Pages/login/vendor/css/core.css" class="template-customizer-core-css" />
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
            padding: 8px 139px;
            border: 1px solid #96A4C5;
            background-color: #96A4C5;
            border-radius: 7px;
            color: #fff !important;
        }

        .selc {
            padding: 9px;
            border-radius: 6px;
            border: 1px solid #cececeb5;
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

        .input-group-text {
            border: none !important;
        }

        .form-control {
            border: none;
        }

        input[type="password"] {
            border: none !important;
        }

        .dropcontri {
            width: 100%;
            padding: 8px 15px;
            border: 1px solid #eeeeee;
            border-radius: 6px;
            color: #7d7d7d;
        }

        .select2-container--default .select2-selection--single .select2-selection__arrow {
            top: 7px !important;
        }

        .select2-container .select2-selection--single .select2-selection__rendered {
            display: block;
            padding: 5px 15px !important;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .select2-container .select2-selection--single {
            height: 39px !important;
        }

        .select2-container--default .select2-selection--single {
            border: 1px solid #e9e9e9 !important;
        }

        .form-control {
            border: 1px solid #e8e8e8 !important;
        }

        .select2-container--default .select2-selection--single .select2-selection__placeholder {
            color: #7b7b7b !important;
        }

        .select2-container {
            max-width: 550px;
            width: 100% !important;
        }
    </style>
    <script>
        function hideLabelAfterTimeout() {
            setTimeout(function () {
                var label = document.getElementById('<%= txtlableer.ClientID %>');
                if (label) {
                    label.style.display = 'none';
                }
            }, 5000); // 5000 milliseconds = 5 seconds
        }
        $(document).ready(function () {
            $('#<%= dropCountri.ClientID %>').select2({
                placeholder: "Chọn Quốc Tịch ....",
                allowClear: true
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h1 class="heading_primary">Đăng Ký</h1>
                    <%--<ul class="breadcrumbs">
                    <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                    <li class="item"><span class="separator"></span></li>
                    <li class="item active">Danh Sách Sự Kiện</li>
                </ul>--%>
                </div>
            </div>
        </div>
        <div class="container-xxl py-5">
            <div class="container">
                <div class="authentication-inner">
                    <div class="card">
                        <div class="card-body">
                            <h2 class="login">Đăng Ký</h2>
                            <asp:Literal ID="litMessage" runat="server" EnableViewState="false" />
                            <div class="mb-3">
                                <div>
                                    <asp:Label ID="txtlableer" runat="server" Text="" Visible="false" ForeColor="Red"> </asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="name" class="form-label">Tên </label>
                                    <asp:TextBox ID="txtname" runat="server" CssClass="form-control" PlaceHolder="Họ Và Tên..." AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="name" class="form-label">Giới Tính</label>
                                    <asp:DropDownList ID="txtgtinh" CssClass="selc w-100" runat="server">
                                        <asp:ListItem Text="Chọn Giới Tính" Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Nam" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Nữ" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Khác" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="date" class="form-label">Ngày Sinh </label>
                                    <asp:TextBox ID="txtngaysinh" runat="server" CssClass="form-control" TextMode="Date" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="phone" class="form-label">Số Điện Thoại</label>
                                    <asp:TextBox ID="txtphone" runat="server" CssClass="form-control" PlaceHolder="Số điện thoại" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="email" class="form-label">Email(Có thể không nhập) </label>
                                    <asp:TextBox ID="txtemail" runat="server" pattern="/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/" TextMode="Email" CssClass="form-control" PlaceHolder="Email" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="address" class="form-label">Địa Chỉ </label>
                                    <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control" PlaceHolder="Địa chỉ" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="address" class="form-label">Quốc Tịch </label>
                                    <br />
                                    <asp:DropDownList ID="dropCountri" runat="server" CssClass="dropcontri"></asp:DropDownList>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="address" class="form-label">Cmnd hoặc Cccd * </label>
                                    <asp:TextBox ID="txtidentifynumber" runat="server" CssClass="form-control" PlaceHolder=" Cccd..." AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3">
                                    <label for="username" class="form-label">Tên Đăng  Nhập</label>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" PlaceHolder="Tên đăng nhập" AutoFocus="true" autocomplete="off"></asp:TextBox>
                                </div>

                                <div class="col-12 col-lg-6 col-sm-6 col-md-6 mb-3 form-password-toggle">
                                    <div class="d-flex justify-content-between">
                                        <label class="form-label" for="password">Mật Khẩu</label>
                                    </div>
                                    <div class="input-group input-group-merge pass">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" PlaceHolder="Mật khẩu" autocomplete="new-password"></asp:TextBox>
                                        <span class="input-group-text cursor-pointer eyes"><i class="bx bx-hide"></i></span>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Yêu cầu nhập mật khẩu" CssClass="text-danger"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                                        ErrorMessage="Mật khẩu phải chứa ít nhất 1 kí tự in hoa, 1 kí tự số, và phải trên 8 kí tự"
                                        CssClass="text-danger"
                                        ValidationExpression="^(?=.*[A-Z])(?=.*\d).{8,}$">
                                    </asp:RegularExpressionValidator>
                                </div>
                                <div class="mb-3 text-center col-12">
                                    <asp:Button ID="btnSignIn" Style="max-width: 359px; border-radius: 7px;" OnClick="btnSave_Click" OnClientClick="return validateDates();" runat="server" CssClass="btn btn-primary d-grid w-100" Text="Đăng Ký" />
                                </div>
                                <p class="col-12 text-center  createac" style="padding: 8px 0px;">
                                    Bạn đã có tài khoản..<a href="Login.aspx" class="text-black font-weight-bold">
                                        Đăng nhập
                                    </a>
                                </p>
                            </div>
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

