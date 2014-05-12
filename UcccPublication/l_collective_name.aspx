<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="l_collective_name.aspx.cs" Inherits="l_collective_name" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                <font color="maroon" size="2" style="font-weight:bold">
                    Collective Names<br /><br />
                </font>
    <table>
        <tr>
            <td align="center">
              <asp:GridView ID="grdCollectiveName" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="l_Collective_Name_id"
                OnRowCancelingEdit="grdCollectiveName_RowCancelingEdit" 
                OnRowDataBound="grdCollectiveName_RowDataBound" 
                OnRowEditing="grdCollectiveName_RowEditing" 
                OnRowUpdating="grdCollectiveName_RowUpdating" 
                OnRowDeleting="grdCollectiveName_RowDeleting" 
                OnRowUpdated="grdCollectiveName_RowUpdated"
                OnRowCommand="grdCollectiveName_RowCommand" 
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
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="l_collective_name_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("l_collective_name_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("l_collective_name_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField ControlStyle-Width="340px" HeaderText="Collective Name" HeaderStyle-HorizontalAlign="Left" SortExpression="collective_name"> 
                    <EditItemTemplate> 
                        <asp:TextBox ID="txtCollectiveName" Width="340px" runat="server" Text='<%# Bind("collective_name") %>'></asp:TextBox> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:TextBox ID="txtNewCollectiveName" Width="340px" runat="server"> </asp:TextBox> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblCollectiveName" runat="server" Text='<%# Bind("collective_name") %>'></asp:Label> 
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
                        <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this collective name?');" ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton> 
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

