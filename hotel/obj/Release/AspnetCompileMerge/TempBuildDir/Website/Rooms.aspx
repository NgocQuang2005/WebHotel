<%@ Page Title="" Language="C#" MasterPageFile="~/Website/Websitehotel.Master" AutoEventWireup="true" CodeBehind="Rooms.aspx.cs" Inherits="hotel.Website.Rooms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Phòng theo loại phòng</title>
    <style>
        .loop-pagination li.active a {
            background-color: #bfecff; /* Highlight color for the active page */
            color: white;
        }

        .loop-pagination li a {
            display: inline-block;
            padding: 5px 10px;
            margin: 0 2px;
            border: 1px solid #ccc;
            border-radius: 3px;
            text-decoration: none;
            color: black;
        }

            .loop-pagination li a:hover {
                background-color: #ddd;
            }

        #prevPage, #nextPage {
            display: inline-block;
            padding: 5px 10px;
            margin: 0 2px;
            border: 1px solid #ccc;
            border-radius: 3px;
            text-decoration: none;
            color: black;
        }

        .textline2 {
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
            display: -webkit-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main-content">
        <div class="page-title">
            <div class="page-title-wrapper" data-stellar-background-ratio="0.5">
                <div class="content container">
                    <h3 class="heading_primary">Phòng</h3>
                    <ul class="breadcrumbs">
                        <li class="item"><a href="Home.aspx">Trang Chủ</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item "><a href="Room.aspx">Loại Phòng</a></li>
                        <li class="item"><span class="separator"></span></li>
                        <li class="item "><a class="active" href="#">Phòng</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="site-content container">
            <div class="rooms-content layout-grid style-02">
                <div class="row" id="roomContainer">
                    <asp:ListView ID="lstViewEmployeeDetails" runat="server">
                        <ItemTemplate>
                            <div class="room col-sm-4 clearfix pb-4 " style="height: 561px;" data-page="1">
                                <div class="room-item" style="height: 100%;">
                                    <div class="room-media">
                                        <a href="<%# "Roomsingle.aspx?id=" + Eval("id").ToString() %>">
                                            <img src='<%# ResolveUrl(Eval("URL").ToString()) %>' style="width: 100%; height: 289px;" alt=""></a>
                                    </div>
                                    <div class="room-summary">
                                        <h3 class="room-title">
                                            <a href="<%# "Roomsingle.aspx?id=" + Eval("id").ToString() %>"><%# Eval("name") %></a>
                                        </h3>
                                        <ul class="room-info">
                                            <li>Tầng <%# Eval("num_floor") %></li>
                                            <li><span class="separator"></span></li>
                                            <li>Số giường: <%# Eval("num_bed") %></li>
                                        </ul>
                                        <div class="line"></div>
                                        <div class="room-meta clearfix">
                                            <div class="price">
                                                <span class="title-price">Thông Tin:</span>
                                                <span class="price_value price_min">
                                                    <div class="textline2"><%# Eval("description") %></div>
                                                </span>
                                            </div>
                                            <%--<div class="rating"><span class="star"></span></div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <ul class="loop-pagination" id="pageNavPosition">
                    <li><a href="#" id="prevPage"><i class="fa fa-angle-left"></i></a></li>
                    <li id="paginationLinks" class="paginationLink"></li>
                    <li><a href="#" id="nextPage"><i class="fa fa-angle-right"></i></a></li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const roomsPerPage = 6;  // Number of rooms displayed per page
            const roomContainer = document.getElementById('roomContainer');
            const rooms = roomContainer.getElementsByClassName('room');
            const paginationContainer = document.getElementById('paginationLinks');
            const prevPageLink = document.getElementById('prevPage');
            const nextPageLink = document.getElementById('nextPage');
            let currentPage = 1;

            function showPage(page) {
                currentPage = page;
                const start = (page - 1) * roomsPerPage;
                const end = start + roomsPerPage;

                Array.from(rooms).forEach((room, index) => {
                    room.style.display = (index >= start && index < end) ? 'block' : 'none';
                });

                Array.from(paginationContainer.children).forEach(link => {
                    link.classList.toggle('active', parseInt(link.querySelector('a').getAttribute('data-page')) === page);
                });

                updateNavigationLinks();
            }

            function createPaginationLinks() {
                paginationContainer.innerHTML = '';
                const totalPages = Math.ceil(rooms.length / roomsPerPage);

                for (let i = 1; i <= totalPages; i++) {
                    const li = document.createElement('li');
                    const a = document.createElement('a');
                    a.href = '#';
                    a.className = 'paginationLink';
                    a.setAttribute('data-page', i);
                    a.textContent = i;

                    li.appendChild(a);
                    paginationContainer.appendChild(li);

                    a.addEventListener('click', function (e) {
                        e.preventDefault();
                        showPage(parseInt(this.getAttribute('data-page')));
                    });
                }
            }

            function updateNavigationLinks() {
                prevPageLink.style.visibility = currentPage > 1 ? 'visible' : 'hidden';
                nextPageLink.style.visibility = currentPage < Math.ceil(rooms.length / roomsPerPage) ? 'visible' : 'hidden';
            }

            prevPageLink.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage > 1) {
                    showPage(currentPage - 1);
                }
            });

            nextPageLink.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentPage < Math.ceil(rooms.length / roomsPerPage)) {
                    showPage(currentPage + 1);
                }
            });

            createPaginationLinks();
            showPage(1);  // Display the first page initially
        });

    </script>


</asp:Content>

