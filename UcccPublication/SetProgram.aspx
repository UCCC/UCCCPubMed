<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="SetProgram.aspx.cs" Inherits="SetProgram" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
            <div class="fullblock" id="inventoryDiv" runat="server" style="text-align:center">
                <font color="maroon" size="2" style="font-weight:bold">
                    Set Program to Publication<br />
                </font>
                <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>                
                <br />
                <table style="width: 52%" align="center">
                    <tr>
                        <td align="left">
                            Start Date:<asp:TextBox id="txtStartDate" Runat="server" Width="71px" /></td>
                        <td align="left">
                    <asp:DropDownList ID="ddlMember" runat="server">
                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            End Date: <asp:TextBox ID="txtEndDate" runat="server" Width="71px"></asp:TextBox>
                        </td>
                        <td align="left">
                    <asp:Button ID="btnGetPublicationlist" runat="server" 
                        onclick="btnGetPublicationlist_Click" Text="List Publications" Width="160px" />
                        </td>
                    </tr>
                    </table>
                    &nbsp;<asp:HiddenField ID="hdnPublicationId" runat="server" />
                <div class="dashedline">
            <asp:GridView ID="gvPublication" 
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                DataKeyNames="publication_program_id"
                OnRowCancelingEdit="gvPublication_RowCancelingEdit" 
                OnRowDataBound="gvPublication_RowDataBound" 
                OnRowCommand="gvPublication_RowCommand" 
                OnRowDeleting="gvPublication_RowDeleting" 
                OnRowEditing="gvPublication_RowEditing" 
                OnRowUpdating="gvPublication_RowUpdating" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" ForeColor="Black" Width="688px"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField HeaderText="PMID" SortExpression="startdate" ControlStyle-Width="80">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%# "http://www.ncbi.nlm.nih.gov/pubmed/" +  Eval("pmid") %>' Text='<%# Bind("pmid") %>'></asp:HyperLink>
                    </ItemTemplate>
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewPmid" Width="80px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID" Visible="false"  HeaderStyle-HorizontalAlign="Left" SortExpression="publication_program_id"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_program_id") %>'></asp:Label>
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("publication_program_id") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Publication" HeaderStyle-HorizontalAlign="Left" SortExpression="publication_id"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblPublication" Width="150px" runat="server" Text='<%# Eval("publication") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Authors" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="authorlist_no_inst"> 
                    <EditItemTemplate> 
                        <asp:Label ID="lblAuthorlist" Width="180px" runat="server" Text='<%# Bind("authorlist_no_inst") %>'></asp:Label> 
                    </EditItemTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblAuthorlist" runat="server" Text='<%# Bind("authorlist_no_inst") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Program" HeaderStyle-HorizontalAlign="Left" SortExpression="l_program_id"> 
                    <EditItemTemplate> 
                        <asp:DropDownList ID="ddlProgram" Width="60px" runat="server" DataTextField="program"
                            DataValueField="l_program_id"> 
                        </asp:DropDownList> 
                    </EditItemTemplate> 
                    <FooterTemplate> 
                        <asp:DropDownList ID="ddlNewProgram" Width="60px" runat="server"> </asp:DropDownList> 
                    </FooterTemplate> 
                    <ItemTemplate> 
                        <asp:Label ID="lblProgram" Width="60px" runat="server" Text='<%# Eval("program") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
                <asp:TemplateField Visible="false" HeaderText="Remov from Program">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chxSelect" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="true" HeaderText="" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <FooterTemplate> 
                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Add"></asp:LinkButton> 
                    </FooterTemplate> 
                    <ControlStyle Width="50px"></ControlStyle>
                    <FooterStyle Width="50px"></FooterStyle>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="50px"></ItemStyle>
                </asp:TemplateField> 
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 

                    <br />

                    <asp:Button ID="btnRemoveSelected" runat="server" 
                        onclick="btnRemoveSelected_Click" Text="Remove Selected from Program" 
                        Visible="False" />

                </div>
            </div>
        </div>
</asp:Content>

