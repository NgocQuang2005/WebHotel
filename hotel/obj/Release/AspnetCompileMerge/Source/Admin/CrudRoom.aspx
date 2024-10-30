<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="CrudRoom.aspx.cs" Inherits="hotel.Admin.CrudRoom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content1" runat="server">
    <style type="text/css">
        .img-thumbnail {
            max-width: 100%;
            height: auto;
        }

        .position-relativse {
            display: inline-block;
        }

        .btn-danger {
            position: absolute;
            top: 5px;
            right: 5px;
            z-index: 1;
            padding: 0px 5px !important;
        }
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
            $('#<%= txttyperoom.ClientID %>').select2({
                placeholder: 'Chọn Loại Phòng...',
                allowClear: true
            });
        });
        $(document).ready(function () {
            $('#<%= SearchTypeRoom.ClientID %>').select2({
                placeholder: 'Chọn Loại Phòng...',
                allowClear: true
            });
        });
        function previewImages(input) {
            if (input.files) {
                var preview = document.getElementById('imagePreview');
                preview.innerHTML = ''; // Xóa bản xem trước trước đó

                Array.from(input.files).forEach((file, index) => {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var img = document.createElement('img');
                        img.style.maxWidth = "150px";
                        img.style.height = "90px";
                        img.src = e.target.result;
                        img.classList.add('img-thumbnail', 'm-1');
                        img.style.width = '100%'; // Đặt chiều rộng hình ảnh

                        var removeBtn = document.createElement('button');
                        removeBtn.innerHTML = 'X';
                        removeBtn.classList.add('btn', 'btn-danger', 'position-absolute');
                        removeBtn.style.top = '5px';
                        removeBtn.style.right = '-5px';
                        removeBtn.style.padding = '0px 5px !important';

                        var imgContainer = document.createElement('div');
                        imgContainer.classList.add('position-relative', 'm-1');
                        imgContainer.style.display = 'inline-block';
                        imgContainer.style.width = '150px'; // Đặt chiều rộng container
                        imgContainer.style.height = 'auto';
                        imgContainer.dataset.index = index;

                        removeBtn.onclick = function () {
                            preview.removeChild(imgContainer);
                            updateFileInput(input, index);
                        };

                        imgContainer.appendChild(img);
                        imgContainer.appendChild(removeBtn);
                        preview.appendChild(imgContainer);
                    };
                    reader.readAsDataURL(file);
                });
            }
        }

        function updateFileInput(input, indexToRemove) {
            var dt = new DataTransfer();
            Array.from(input.files).forEach((file, index) => {
                if (index !== indexToRemove) {
                    dt.items.add(file);
                }
            });
            input.files = dt.files;

            // Đặt lại input nếu không còn tệp nào
            if (input.files.length === 0) {
                input.value = ''; // Đặt lại input để hiển thị "No file chosen"
            }
        }

    </script>
    <div class="container py-2">
        <div id="form_data" visible="false" runat="server" class="content_crud bdlayout" style="padding-bottom: 50px;">
            <h2>
                <asp:Label ID="txth2" ForeColor="White" Font-Bold="true" Visible="true" runat="server" Text="Thêm Phòng"></asp:Label>
            </h2>
            <div style="display: flex; gap: 1.6rem !important; align-items: center; margin: 20px;">
                <div>
                    <asp:FileUpload ID="FileUploadImage" runat="server" multiple="multiple" onchange="previewImages(this);" />
                </div>
                <div id="imagePreview" class="d-flex flex-wrap">
                </div>
            </div>
            <hr style="border-bottom: 1px solid #e6e6e6" />
            <div class="position-relative" style="padding: 0px 15px;">
                <div class="formedit">
                    <div class="formedit_div">
                        <div class="formedit_lable">Trạng Thái </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtactive" runat="server" />

                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Tên Phòng</div>
                        <asp:TextBox ID="txtname" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Tầng </div>
                        <asp:TextBox ID="txtnumfllor" runat="server" CssClass="crud_iput"></asp:TextBox>

                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Diện Tích</div>
                        <asp:TextBox ID="txtspread" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>

                    <div class="formedit_div">
                        <div class="formedit_lable">Số giường</div>
                        <asp:TextBox ID="txtnumbed" runat="server" CssClass="crud_iput"></asp:TextBox>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Loại Phòng </div>
                        <asp:DropDownList ID="txttyperoom" CssClass="selc w-100" runat="server"></asp:DropDownList>
                    </div>

                    <div class="formedit_div">
                        <div id="editor">
                            <div class="formedit_lable">Ghi Chú</div>
                            <div id="quillEditor" class="w-100" style="height: 200px;"></div>
                            <asp:TextBox ID="txtdescription" Style="display: none;" runat="server" CssClass="crud_iput"></asp:TextBox>
                        </div>
                    </div>
                    <div class="formedit_div">
                        <div class="formedit_lable">Tình Trạng </div>
                        <div class="stast">
                            <asp:CheckBox ID="txtstt" runat="server" />
                        </div>
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
            <h3 class="position-relative">Danh sách phòng 
                <asp:Button ID="btnNew" OnClick="btnNew_Click" CssClass="position-absolute btn btn-success asnew text-white" runat="server" Text="Tạo Mới" />
            </h3>
            <div class="search">
                <asp:TextBox ID="SearchName" runat="server" placeholder="Nhập tên phòng ......" CssClass="search_input" />
                <%--<asp:CheckBox ID="SearchActive" runat="server" /> Active--%>
                <asp:DropDownList ID="SearchActiveDropdown" CssClass="selc" runat="server">
                    <asp:ListItem Text="Tất Cả" Value="" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hoạt Động" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Không Hoạt Động" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="SearchTypeRoom" runat="server" CssClass="selc"></asp:DropDownList>
                <asp:LinkButton ID="btnSearchr" runat="server" CssClass=" btn btn-primary btnSearchr" Text="Search" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hfSelectedRecord" runat="server" />
            <asp:HiddenField ID="hfSearchNameClientID" runat="server" />
            <div class="table-responsive " style="padding-bottom: 35px;">
                <asp:ListView ID="lstViewRoomDetails" OnItemCommand="lstViewRoomDetails_ItemCommand" runat="server">
                    <LayoutTemplate>
                        <table id="tblEmpDetails">
                            <thead>
                                <tr class="altRow">
                                    <th class="rb" width="30px"><b>STT</b></th>
                                    <th class="rb" width="40px"><b>Trạng Thái</b></th>
                                    <th class="rb" width="130px"><b>Tên</b></th>
                                    <th class="rb" width="40px"><b>Tầng</b></th>
                                    <th class="rb" width="40px"><b>Số giường</b></th>
                                    <th class="rb"><b>Miêu Tả</b></th>
                                    <th class="rb" width="140px"><b>Loại Phòng</b></th>
                                    <th class="rb" width="90px"><b>Tình Trạng</b></th>
                                    <th class="b" width="130px"><b>Thao Tác</b></th>
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
                            <td class="trb"><%# Eval("name") %></td>
                            <td class="trb"><%# Eval("num_floor") %></td>
                            <td class="trb text-right"><%# Eval("num_bed") %></td>
                            <td class="trb text-left"><%# Eval("description") %></td>
                            <td class="trb"><%# Eval("typename") %></td>
                            <td class="trb"><%# (Eval("state") != DBNull.Value && Convert.ToBoolean(Eval("state")))?"Đã Đặt":"Chưa Đặt" %></td>
                            <td class="tb">
                                <asp:LinkButton ID="btnEdit" CommandName="Edt" CommandArgument='<%# Eval("id") %>' runat="server"><i class="fa fa-edit"></i>Sửa</asp:LinkButton>&nbsp;|&nbsp;
                                <asp:LinkButton OnClientClick="return confirm('Are you sure to delete record?')" ID="imgBtnDelete" CommandName="Del" CommandArgument='<%# Eval("id") %>' runat="server"><i style="color: red" class="fa fa-trash"></i>Xóa</asp:LinkButton>

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
