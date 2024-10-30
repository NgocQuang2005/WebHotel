<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="hotel.Admin.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Home</title>

    <style>
        .userlog {
            background: #86abcf;
            opacity: 0.9;
            color: #fff;
            padding: 8px 23px;
            border-radius: 5px 5px 0px 0px;
            font-size: 13px;
        }

        .anhlogo {
            padding: 0px;
            width: 49px;
            height: 49px;
            border-radius: 6px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <div class="container">
        <div class="row">
            <div class="container-xxl flex-grow-1 container-p-y">
                <div class="row">
                    <div class="col-lg-8 col-md-8 pr-1 mb-4 order-0 ">
                        <div class="card mb-3">
                            <div class="d-flex align-items-end row">
                                <div class="col-sm-7">
                                    <div class="card-body">
                                        <h5 class="card-title text-primary">Xin Chào
                                            <asp:Literal ID="litUserName" runat="server"></asp:Literal>! 🎉</h5>
                                        <p class="mb-4">
                                            Khách sạn Lionn chúc Bạn 1 ngày mới thật vui vẻ nhé!!!
                                        </p>

                                        <%--<a href="" class="btn btn-sm btn-outline-primary"></a>--%>
                                    </div>
                                </div>
                                <div class="col-sm-5 text-center text-sm-left">
                                    <div class="card-body pb-0 px-0 px-md-4">
                                        <img src="../App_Themes/Admin_Pages/Layout/assets/img/illustrations/man-with-laptop-light.png"
                                            height="140"
                                            alt="View Badge User"
                                            data-app-dark-img="illustrations/man-with-laptop-dark.png"
                                            data-app-light-img="illustrations/man-with-laptop-light.png" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="row row-bordered g-0">
                                <div class="col-md-7">
                                    <div class="card h-100">
                                        <div class="card-header" style="padding: 18px 24px 10px 24px;">
                                            <ul class="nav nav-pills" role="tablist">
                                                <li class="nav-item text-primary font-weight-bold" style="font-weight: 500; font-size: 17px;">
                                                    Thống Kê Nguồn Thu Nhập 
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="card-body px-0" style="padding-bottom: 2px;">
                                            <div class="tab-content p-0" style="padding-bottom: 10px !important;">
                                                <div class="tab-pane fade show active" id="navs-tabs-line-card-income" role="tabpanel">
                                                    <div class="d-flex p-4 pt-3" style="padding: 10px 24px 10px 24px !important;">
                                                        <div class="avatar flex-shrink-0 me-3">
                                                            <img src="../App_Themes/Admin_Pages/Layout/assets/img/icons/unicons/wallet.png" alt="User" />
                                                        </div>
                                                        <div>
                                                            <small class="text-muted d-block">% Doanh Thu</small>
                                                            <div class="d-flex align-items-center">
                                                                <h6 class="mb-0 me-1">$459.10</h6>
                                                                <small class="text-success fw-medium">
                                                                    <i class="bx bx-chevron-up"></i>
                                                                    42.9%
                                                            </small>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="incomeChart"></div>
                                                    <div class="d-flex justify-content-center pt-4 gap-2" style="padding-top: 10px !important;">
                                                        <div class="flex-shrink-0">
                                                            <div id="expensesOfWeek"></div>
                                                        </div>
                                                        <div>
                                                            <p class="mb-n1 mt-1">Expenses This Week</p>
                                                            <small class="text-muted">$39 less than last week</small>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="card-body">
                                        <div class="text-center">
                                            <div class="dropdown">
                                                <button class="btn btn-sm btn-outline-primary dropdown-toggle"
                                                    type="button"
                                                    id="growthReportId"
                                                    data-bs-toggle="dropdown"
                                                    aria-haspopup="true"
                                                    aria-expanded="false">
                                                    2022
             
                                                </button>
                                                <div class="dropdown-menu dropdown-menu-end" aria-labelledby="growthReportId">
                                                    <a class="dropdown-item" href="javascript:void(0);">2021</a>
                                                    <a class="dropdown-item" href="javascript:void(0);">2020</a>
                                                    <a class="dropdown-item" href="javascript:void(0);">2019</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="growthChart" style="padding: 12px 0px;"></div>
                                    <div class="text-center fw-medium pt-5 mb-2">62% Company Growth</div>

                                    <div class="d-flex px-xxl-4 px-lg-2 p-4 gap-xxl-3 gap-lg-1 gap-3 justify-content-between"
                                        style="padding: 22px 8px 21px 8px !important; }">
                                        <div class="d-flex">
                                            <div class="me-2">
                                                <span class="badge bg-label-primary p-2"><i class="bx bx-dollar text-primary"></i></span>
                                            </div>
                                            <div class="d-flex flex-column">
                                                <small>2022</small>
                                                <h6 class="mb-0">$32.5k</h6>
                                            </div>
                                        </div>
                                        <div class="d-flex">
                                            <div class="me-2">
                                                <span class="badge bg-label-info p-2"><i class="bx bx-wallet text-info"></i></span>
                                            </div>
                                            <div class="d-flex flex-column">
                                                <small>2021</small>
                                                <h6 class="mb-0">$41.2k</h6>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4 pl-1 ">
                        <div class="bdlayout">
                            <h6 class="userlog">Người Đăng Nhập Cuối Cùng</h6>
                            <div class="container ">
                                <ul style="padding-left: 0px; list-style-type: none;">
                                    <asp:Repeater ID="rptLastLoggedInUsers" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <div class="row">
                                                    <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' alt="" class="anhlogo col-lg-3 col-md-3 col-sm-6 " />
                                                    <div class="col-lg-9 col-md-9 col-sm-6">
                                                        <h5 style="margin: 0; font-weight: bold; font-size: 17px; color: black;">
                                                            <%# Eval("FullName") %>
                                                        </h5>
                                                        <span style="color: red; font-size: 14px;"><%#(Eval("role") != DBNull.Value && int.TryParse(Eval("role").ToString(), out int roleValue) ? 
                                                               (roleValue == 1 ? "Quản Lý" : 
                                                               (roleValue == 2 ? "Nhân Viên" :
                                                               (roleValue == 3 ? "Khách Hàng" : "Chưa cập nhật"))) : "Chưa cập nhật") %></span>
                                                        <div style="font-size: 11px;"><b>Đăng nhập lần cuối: </b><%# Eval("LastLoginTime", "{0:MM/dd/yyyy hh:mm tt}") %></div>
                                                    </div>
                                                </div>
                                            </li>
                                            <hr />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <%--<div class="col-12 col-lg-8 order-2 order-md-3 order-lg-2 mb-4">
                    </div>--%>
                </div>
            </div>
            <div class="footer">
                <div class="footer-left">
                    <span>&copy; 2024. Admin Hotel Lionn. All Rights Reserved.</span>
                </div>
                <div class="footer-right">
                    <span>Designed by: <a href="https://github.com/NgocQuang2005" style="text-decoration: none;">NgocQuang</a></span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
