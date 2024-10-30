<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CompanyInformation.aspx.cs" Inherits="hotel.Admin.CompanyInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Thông tin công ty</title>
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
            var aspTextBox = document.getElementById('<%= txtdescription.ClientID %>');

            // Set initial value from the ASP.NET TextBox to Quill editor
            quill.root.innerHTML = aspTextBox.value;

            // Sync Quill content to ASP.NET TextBox on form submit
            document.querySelector("form").onsubmit = function () {
                aspTextBox.value = quill.root.innerHTML;
            };
        });
        $(document).ready(function () {
            $('#<%= ddlCountries.ClientID %>').select2({
                placeholder: "Chọn Quốc Tịch ....",
                allowClear: true
            });
        });
    </script>
    <div class="container">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px;">

            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Công Ty"></asp:Label>
            </h2>
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtacti" runat="server" />
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Type</div>
                        <asp:TextBox ID="txttype" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Tên Công Ty</div>
                        <asp:TextBox ID="txtname" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Số Điện Thoại</div>
                        <asp:TextBox ID="txtphone" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Email</div>
                        <asp:TextBox ID="txtemail" runat="server" TextMode="Email" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Địa Chỉ</div>
                        <asp:TextBox ID="txtaddress" runat="server" CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Thành Phố</div>
                        <asp:TextBox ID="txtcity" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Quốc Gia</div>
                        <asp:DropDownList ID="ddlCountries" runat="server" CssClass="crud_iput"></asp:DropDownList>
                    </div>
                    <div class="formedit_div">
                        <div id="editor">
                            <div class="formedit_lable">Miêu Tả</div>
                            <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                            <asp:TextBox ID="txtdescription" runat="server" Style="display: none;" TextMode="MultiLine" CssClass="crud_iput"></asp:TextBox>
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable ">Ngày Thành Lập</div>
                        <asp:TextBox ID="txtesdate" runat="server" TextMode="Date" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <asp:Label ID="txtlableer" ForeColor="Red" Font-Bold="true" Visible="false" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="buttonEvent">
                    <asp:Button Text="Lưu" OnClick="btnSave_Click" ID="btnSave" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                    <asp:Button Text="Cập Nhật" ID="btnUpdate" OnClick="btnUpdate_Click" Visible="false" runat="server" CssClass="btn btn-outline-primary"></asp:Button>
                    <asp:Button Text="Hủy" ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-outline-danger"></asp:Button>

                </div>
            </div>
        </div>
        <div id="list_data" runat="server" class="content_cruds bdlayout">
            <h3 class="position-relative">Thông tin công ty
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <p>Tìm kiếm tên công ty: </p>
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên công ty ......" CssClass="search_input" />
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

                <asp:ListView ID="lstViewCompanyDetails" OnItemCommand="lstViewCompanyDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="150px"><b>Tên Công Ty</b></th>
                                    <th class="rb"><b>Địa Chỉ</b></th>
                                    <th class="rb" width="150px"><b>Email</b></th>
                                    <th class="rb" width="100px"><b>Ngày Thành Lập</b></th>
                                    <th class="rb" width="120px"><b>Quốc Gia</b></th>
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
                            <td class="trb"><%# (Eval("active") != DBNull.Value && Convert.ToBoolean(Eval("active")))?"<i style='color:green' class='fa fa-check' ></i>":"<i style='color:red' class='fa fa-times'></i>" %></td>
                            <td class="trb"><%# Eval("name") %></td>
                            <td class="trb text-center"><%# Eval("address") %></td>
                            <td class="trb"><%# Eval("email") %></td>
                            <td class="trb"><%# Eval("establish_date") != DBNull.Value?Convert.ToDateTime(Eval("establish_date")).ToString("dd/MM/yyyy"):"-.-"%></td>
                            <td class="trb"><%# Eval("country") %></td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i> Sửa</asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color:red" class="fa fa-trash"></i> Xóa</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div id="pageNavPosition" class="text-right"></div>
        </div>
    </div>
    <script type="text/javascript">
        var pager = new Pager('tblEmpDetails', 5);
        pager.init();
        pager.showPageNav('pager', 'pageNavPosition');
        pager.showPage(1);
    </script>
</asp:Content>
