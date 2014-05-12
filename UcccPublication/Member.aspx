<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Member.aspx.cs" Inherits="Member" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                <font color="maroon" size="2" style="font-weight:bold">
                    Member<br /><br />
                </font>
    <table>
        <tr>
            <td align="center">
              <asp:GridView ID="gvMember" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="client_id"
                OnRowCancelingEdit="gvMember_RowCancelingEdit" 
                OnRowDataBound="gvMember_RowDataBound" 
                OnRowEditing="gvMember_RowEditing" 
                OnRowUpdating="gvMember_RowUpdating" 
                OnRowUpdated="gvMember_RowUpdated"
                OnRowCommand="gvMember_RowCommand" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="Member ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("client_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("client_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="140px" HeaderText="Last Name" HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtLastName" Width="140px" runat="server" Text='<%# Bind("last_name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewLastName" Width="140px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("last_name") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="140px" HeaderText="First Name" HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtFirstName" Width="140px" runat="server" Text='<%# Bind("first_name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewFirstName" Width="140px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("first_name") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="40px" HeaderText="Mid Initial" HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtMi" Width="40px" runat="server" Text='<%# Bind("mi") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewMi" Width="40px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblMi" runat="server" Text='<%# Bind("mi") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" ControlStyle-Width="60px" FooterStyle-Width="60px" HeaderStyle-Width="60px" ItemStyle-Width="60px" HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <EditItemTemplate> 
                        <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Save"></asp:LinkButton> 
                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Add"></asp:LinkButton> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
                    </ItemTemplate> 
                    <ControlStyle Width="60px"></ControlStyle>
                    <FooterStyle Width="60px"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="60px"></ItemStyle>
                </asp:TemplateField> 
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 

            </td>
          </tr>
        </table>
                </div>
                <div class="dashedline">
                </div>
                <!-- begin inventory item -->
            </div>
        </div>
        <div class="clear2column">
    </div>



</asp:Content>

