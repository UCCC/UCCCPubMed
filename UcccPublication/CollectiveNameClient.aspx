<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="CollectiveNameClient.aspx.cs" Inherits="CollectiveNameClient" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                <font color="maroon" size="2" style="font-weight:bold">
                    Collective Name Member<br /><br />
                </font>
    <table>
        <tr>
            <td align="center">
              <asp:GridView ID="gvCollectiveNameClient" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="collective_name_client_id,l_collective_name_id, client_id"
                OnRowCancelingEdit="gvCollectiveNameClient_RowCancelingEdit" 
                OnRowDataBound="gvCollectiveNameClient_RowDataBound" 
                OnRowEditing="gvCollectiveNameClient_RowEditing" 
                OnRowUpdating="gvCollectiveNameClient_RowUpdating" 
                OnRowUpdated="gvCollectiveNameClient_RowUpdated"
                OnRowCommand="gvCollectiveNameClient_RowCommand" 
                OnRowDeleting="gvCollectiveNameClient_RowDeleting" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black" Width="431px"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="collective_name_client_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("collective_name_client_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("collective_name_client_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Collective Name" HeaderStyle-HorizontalAlign="Left" SortExpression="l_collective_name_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlCollectiveName" Width="200px" runat="server" DataTextField="collective_name" 
                            DataValueField="l_collective_name_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewCollectiveName" Width="200px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblCollectiveName" Width="200px" runat="server" Text='<%# Eval("collective_name") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Member" HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlMember" Width="120px" runat="server" DataTextField="client" 
                            DataValueField="client_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewMember" Width="120px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblMember" Width="120px" runat="server" Text='<%# Eval("client") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="true" HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <FooterTemplate> 
                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Add"></asp:LinkButton> 
                    </FooterTemplate> 
                    <ItemTemplate> 
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

