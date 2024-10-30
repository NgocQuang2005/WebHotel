<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AccountSettings.aspx.cs" Inherits="hotel.Admin.AccountSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Cài đặt tài khoản</title>
    <style>
        h3 {
            color: #9E6752;
        }

        .accountst {
            padding: 22px 18px;
        }

        .accountst_content {
            display: flex;
            align-items: center;
            padding: 0px 0px 10px 0px;
            justify-content: space-between;
        }

        .accountst_lable {
            font-size: 16px;
            color: #99CDD8;
        }

        .accountst_input {
            width: 553px;
            padding: 5px 13px;
            border-radius: 6px;
            border: 1px solid;
        }

        .accountst_bt {
            border: 1px solid #375b8b;
            color: black;
            padding: 6px 13px;
            border-radius: 8px;
            margin-right: 16px;
            background-color: white;
        }

            .accountst_bt:hover {
                background-color: #375b8b;
                color: #fff;
            }

        .accountst_a {
            border: 1px solid #f3b699;
            color: black;
            padding: 6px 13px;
            border-radius: 8px;
            font-size: 13px;
        }

            .accountst_a:hover {
                background-color: #f3b699;
                color: #fff;
            }

        .toggle-password {
            cursor: pointer;
            position: absolute;
            right: 27px;
            top: 61%;
            transform: translateY(-50%);
            padding: 2px 5px;
            border: 1px solid #86abcf;
            border-radius: 6px;
        }

            .toggle-password:hover {
                background-color: #86abcf;
                color: #fff;
            }

        .showpass {
            position: absolute;
            /* left: 138px; */
            right: 11px;
            color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <div class="container" style="display: flex; justify-content: center;">
        <div class=" bdlayout" style="margin: 40px 50px; padding: 20px 15px 40px 15px; max-width: 753px; width: 100%; position: relative;">
            <h3 style="margin: -21px -15px 0px -15px !important;">Cài Đặt Tài Khoản</h3>
            <div class="accountst">
                <div class="accountst_content">
                    <label class="accountst_lable ">Tên Đăng Nhập: </label>
                    <asp:TextBox ID="txtusername" runat="server" CssClass="accountst_input " />
                </div>
                <div class="accountst_content position-relative">
                    <label class="accountst_lable ">Mật Khẩu: </label>
                    <asp:TextBox ID="txtpassword" TextMode="Password" runat="server" CssClass="accountst_input " />
                    <asp:LinkButton ID="btnShowPass" runat="server" CssClass="showpass" OnClientClick="togglePassword(); return false;"> 
                        <i class="fas fa-eye"></i>
                    </asp:LinkButton>
                </div>
                <div style="display: flex; position: absolute; right: 58px;">
                    <asp:Button Text="Cập Nhật" ID="btnUpdate" runat="server" CssClass="accountst_bt" OnClick="btnUpdate_Click"></asp:Button>
                    <a href="Home.aspx" class="btn btn-outline-danger" style="text-decoration: none;">Hủy </a>
                </div>
                <asp:Label ID="txtlableer" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
    <script>
        function togglePassword() {
            var passwordField = document.getElementById('<%= txtpassword.ClientID %>');
            var toggleButton = document.getElementById('<%= btnShowPass.ClientID %>').firstElementChild;

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleButton.className = "fas fa-eye-slash";
            } else {
                passwordField.type = "password";
                toggleButton.className = "fas fa-eye";
            }
        }
        function hideLabelAfterTimeout() {
            setTimeout(function () {
                var label = document.getElementById('<%= txtlableer.ClientID %>');
                if (label) {
                    label.style.display = 'none';
                }
            }, 5000); // 5000 milliseconds = 5 seconds
        }
    </script>

</asp:Content>
