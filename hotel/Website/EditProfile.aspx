<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="hotel.Website.EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chỉnh Sửa Hồ Sơ</title>
    <style>
        .edittt {
            padding: 20px 15px 50px 15px;
            margin: 40px 20px;
        }

        h3 {
            color: #657166;
        }

        .formedit {
            display: flex;
            justify-content: space-around;
            margin-bottom: 25px;
        }

        .form-control {
            width: 100%;
            padding: 8px 16px;
            border-radius: 6px;
        }

        .formedit_div {
            width: 450px;
        }

        .formedit_lable {
            padding-bottom: 7px;
            font-size: 16px;
            color: #86abcf;
        }

        .edit_bt {
            border: 1px solid #375b8b;
            color: black;
            padding: 6px 13px;
            border-radius: 8px;
            margin-right: 16px;
            background-color: white;
        }

            .edit_bt:hover {
                background-color: #375b8b;
                color: #fff;
            }

        .edit_a {
            border: 1px solid #f3b699;
            color: black;
            padding: 6px 13px;
            border-radius: 8px;
            font-size: 13px;
        }

            .edit_a:hover {
                background-color: #f3b699;
                color: #fff;
            }

        .imguser {
            border-radius: 5px;
            width: 100px;
            height: 100px;
        }

        h3 {
            padding: 9px 15px;
            background: #86abcf;
            opacity: 0.5;
            color: #fff !important;
            border-radius: 11px 11px 0px 0px;
            margin: -7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
            <div class="content container">
                <h1 class="heading_primary">Blog Hotel Lionn</h1>
                <ul class="breadcrumbs">
                    <li class="item"><a href="Home.aspx">Home</a></li>
                    <li class="item"><span class="separator"></span></li>
                    <li class="item active">Blog </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="bdlayout edittt">
            <h3 style="margin: -21px -15px 0px -15px !important;"><b>Chỉnh sửa thông tin</b></h3>
            <hr style="border-bottom: 1px solid #e6e6e6" />
            <div>
                <div class="formedit">
                    <div class="formedit_div">
                        <label for="firstName" class="formedit_lable">Tên</label>
                        <asp:TextBox ID="txtname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <label for="firstName" class="formedit_lable">Email</label>
                        <asp:TextBox ID="txtemail" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="formedit">
                    <div class="formedit_div">
                        <label for="firstName" class="formedit_lable">Số điện thoại</label>
                        <asp:TextBox ID="txtphone" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <label for="firstName" class="formedit_lable">Địa chỉ</label>
                        <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="formedit">
                    <div class="formedit_div">
                        <label for="firstName" class="formedit_lable mr-1">Ngày sinh</label>
                        <asp:TextBox ID="txtbirthday" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                    </div>
                </div>
                <div style="display: flex; position: absolute; right: 190px; width: 159px; justify-content: space-between;">
                    <asp:Button Text="Cập Nhật" ID="btnUpdate" runat="server" CssClass="btn btn-outline-primary" OnClick="btnUpdate_Click"></asp:Button>
                    <a href="Home.aspx" class="btn btn-outline-danger" style="text-decoration: none;">Hủy </a>
                </div>
                <asp:Label ID="txtlableer" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
    <script>
        <%--function previewImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgProfile.ClientID %>').src = e.target.result;
                };
                reader.readAsDataURL(input.files[0]);
            }
        }--%>
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
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server"></asp:Content>

