<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="hotel.Website.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quên mật khẩu</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content ">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h1 class="heading_primary">Quên mật khẩu</h1>
                </div>
            </div>
        </div>
        <div class="forgot-password container my-3">
            <h2>Forgot Password</h2>
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
            <asp:Panel ID="pnlEmail" runat="server">
                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:Button ID="btnSendCode" runat="server" Text="Send Code" OnClick="btnSendCode_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlCode" runat="server" Visible="false">
                <asp:Label ID="lblCode" runat="server" Text="Verification Code:"></asp:Label>
                <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                <asp:Button ID="btnVerifyCode" runat="server" Text="Verify Code" OnClick="btnVerifyCode_Click" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
</asp:Content>
