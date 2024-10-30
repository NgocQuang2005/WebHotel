<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeBehind="AuditLog.aspx.cs" ValidateRequest="false" Inherits="hotel.Admin.AuditLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Nhật kí kiểm toán</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
        //Phân Trang Cho Table

        function hideLabelAfterTimeout() {
            setTimeout(function () {
                var label = document.getElementById('<%= txtlableer.ClientID %>');
                if (label) {
                    label.style.display = 'none';
                }
            }, 5000); // 5000 milliseconds = 5 seconds
        }
        document.addEventListener("DOMContentLoaded", function () {
            // Initialize Quill editor
            const quill = new Quill('#quillEditor', {
                theme: 'snow'
            });

            // Reference to the ASP.NET TextBox control
            var aspTextBox = document.getElementById('<%= txtjsondata.ClientID %>');

            // Set initial value from the ASP.NET TextBox to Quill editor
            quill.root.innerHTML = aspTextBox.value;

            // Sync Quill content to ASP.NET TextBox on form submit
            document.querySelector("form").onsubmit = function () {
                aspTextBox.value = quill.root.innerHTML;
            };
        });
    </script>
    <div class="container py-2">
        <div id="form_data" runat="server" visible="false" class="content_crud bdlayout">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Mới Audit_log"></asp:Label>
            </h2>
            <%--<asp:LinkButton ID="btnback" CssClass="btn btn-outline-secondary m-2" Visible="false" runat="server" OnClick="btnback_Click"><i class="fas fa-chevron-left"></i></asp:LinkButton>--%>
            <div>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <b>Trạng thái :</b>
                            </td>
                            <td class="stast">
                                <asp:CheckBox ID="txtacti" runat="server" />
                            </td>
                        </tr>
                        <tr id="editor">
                            <td>
                                <b>Nội Dung :</b>
                            </td>
                            <td>
                                <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                                <asp:TextBox ID="txtjsondata" runat="server" TextMode="MultiLine" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Tên Bảng:</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txttablename" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Việc làm:</b>
                            </td>
                            <td>
                                <asp:TextBox ID="txtoperation" runat="server" CssClass="crud_iput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>website_click :</b>
                            </td>
                            <td class="stast">
                                <asp:CheckBox ID="txtwebclick" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="txtlableer" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button Text="Lưu" OnClick="btnSave_Click" ID="btnSave" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                                <asp:Button Text="Cập Nhật" ID="btnUpdate" OnClick="btnUpdate_Click" Visible="false" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                                <asp:Button Text="Hủy" ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-outline-danger"></asp:Button>

                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Nhật kí các hành động kiểm toán ...
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm Table: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập theo tên table ......" CssClass="search_input" />
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive text-nowrap " style="padding-bottom: 35px;">

                <asp:ListView ID="lstViewAuditDetails" OnItemCommand="lstViewAuditDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb"><b>Nội Dung</b></th>
                                    <th class="rb" width="80px"><b>Tên Bảng</b></th>
                                    <th class="rb" width="105px"><b>Việc làm</b></th>
                                    <th class="rb" width="40px"><b>Web Click</b></th>
                                    <th class="b" width="150px"><b>Thao Tác</b></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr id="itemPlaceholder" runat="server"></tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr class='<%# (Container.DataItemIndex + 1) % 2 == 0 ? "altrow" : "normalrow" %>'>
                            <td class="trb"><%# Container.DataItemIndex + 1 %></td>
                            <td class="trb"><%# (Eval("active") != DBNull.Value && Convert.ToBoolean(Eval("active")))?"<i style='color:green' class='fa fa-check' ></i>":"<i style='color:red'  class='fa fa-times'></i>" %></td>
                            <td class="trb text-center"><%# Eval("jsondata") %></td>
                            <td class="trb"><%# Eval("table_name") %></td>
                            <td class="trb"><%# Eval("operation") %></td>
                            <td class="trb"><%# (Eval("website_click") != DBNull.Value && Convert.ToBoolean(Eval("website_click")))?"Có":"Không" %></td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i> Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color:red" class="fa fa-trash"></i> Xóa</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="pageNavPosition" class="text-right pointer"></div>
        </div>
    </div>
    <script type="text/javascript">
        var pager = new Pager('tblEmpDetails', 5);
        pager.init();
        pager.showPageNav('pager', 'pageNavPosition');
        pager.showPage(1);
    </script>
</asp:Content>
