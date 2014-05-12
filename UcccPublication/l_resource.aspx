<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="l_resource.aspx.cs" Inherits="l_resource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                <font color="maroon" size="2" style="font-weight:bold">
                    Shared Resources<br /><br />
                </font>
    <table>
        <tr>
            <td align="center">
              <asp:GridView ID="grdResource" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="l_resource_id"
                OnRowCancelingEdit="grdResource_RowCancelingEdit" 
                OnRowDataBound="grdResource_RowDataBound" 
                OnRowEditing="grdResource_RowEditing" 
                OnRowUpdating="grdResource_RowUpdating" 
                OnRowUpdated="grdResource_RowUpdated"
                OnRowCommand="grdResource_RowCommand" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="Select" Visible="false">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chxSelect" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Resource ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="l_resource_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("l_resource_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("l_resource_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="340px" HeaderText="Resource" HeaderStyle-HorizontalAlign="Left" SortExpression="description"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtDescription" Width="340px" runat="server" Text='<%# Bind("description") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewDescription" Width="340px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("description") %>'></asp:Label> 
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

