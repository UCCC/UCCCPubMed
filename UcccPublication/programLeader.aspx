<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="programLeader.aspx.cs" Inherits="programLeader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <asp:Label runat="server" ForeColor="Red" ID="ErrorMessage"></asp:Label>                
            <div class="fullblock" id="inventoryDiv" runat="server">
                <div class="inventorycrumbs">
                <font color="maroon" size="2" style="font-weight:bold">
                    Program Leaders<br /><br />
                </font>
    <table>
        <tr>
            <td align="center">
              <asp:GridView ID="gvProgramLeader" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="program_leader_id,l_program_id, client_id"
                OnRowCancelingEdit="gvProgramLeader_RowCancelingEdit" 
                OnRowDataBound="gvProgramLeader_RowDataBound" 
                OnRowEditing="gvProgramLeader_RowEditing" 
                OnRowUpdating="gvProgramLeader_RowUpdating" 
                OnRowUpdated="gvProgramLeader_RowUpdated"
                OnRowCommand="gvProgramLeader_RowCommand" 
                OnRowDeleting="gvProgramLeader_RowDeleting" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black" Width="431px"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="program_leader_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("program_leader_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("program_leader_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Program" HeaderStyle-HorizontalAlign="Left" SortExpression="l_program_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlProgram" Width="200px" runat="server" DataTextField="program" 
                            DataValueField="l_program_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewProgram" Width="200px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblProgram" Width="200px" runat="server" Text='<%# Eval("program") %>'></asp:Label> 
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
                        <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this program leader?');" ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton> 
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

