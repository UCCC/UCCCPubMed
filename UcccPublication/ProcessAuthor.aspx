<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="ProcessAuthor.aspx.cs" Inherits="ProcessAuthor" %>

<%@ Register TagPrefix="OrganMap" Namespace="OrganMapSite" %>
<%@ Register TagPrefix="OrganMap" TagName="LoginBanner" Src="LoginBanner.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
     <link type="text/css" rel="Stylesheet" href="pubsite.css" />
    <asp:HiddenField ID="hdnPubCnt" runat="server" />
    <div id="body">
        <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red"></asp:Label>
        <table style="width: 99%" align="center">
            <tr>
                <td align="center" style="height: 35px">
                    <font color="maroon" size="2" style="font-weight:bold">
                    Process Author&#39;s Membership<br />
                </font>
                </td>
            </tr>
            <tr>
                <td align="left">
                System will check author's initials to see the author might be our member and assign it member ID.   
                    You can edit membership below.<br /><br />  
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnProcessAuthorList" runat="server" 
                        onclick="btnProcessAuthorList_Click" Text="Process Membership by System" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    &nbsp;</td>
            </tr>
        </table>
        <table style="width: 100%" border="1">
            <tr>
                <td align="center" valign="top" colspan="2">
                    <font color="maroon" size="2" style="font-weight:bold">
                    Manually
                    Edit Author&#39;s Membership<br />
                    <br /></font>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" colspan="2">
                We can process the persons:
                    <ul>
                    <li>They might have similar last names and first namse with our members. <br /></li>
                    <li>They might be the members with slight different name appearence,<br /></li>
                    <li>They might be authors together with our members in same publication, <br /></li>
                    <li>They might be totally other persons with the similar names with our members.<br /></li>
                    </ul>
                    Here we can check one by one. <br /><br />

                    Start Date:<asp:TextBox runat="server" ID="txtStartDate"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    End Date:<asp:TextBox runat="server" ID="txtEndDate"></asp:TextBox>                            
                    <br />
                    or&nbsp;&nbsp;<asp:CheckBox ID="chxAllDate" runat="server" Text="All date" 
                        AutoPostBack="True" oncheckedchanged="CheckAllDates" />
                    &nbsp;(To delete authors, check this box.)<br />
                </td>
            </tr>
            <tr>
                <td valign="top" width="25%">
                    Select start letter of last name to list similar names:<br />
                    <br />
                    <asp:LinkButton id="btnA" Text="A" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnB" Text="B" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnC" Text="C" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnD" Text="D" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnE" Text="E" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnF" Text="F" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnG" Text="G" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnH" Text="H" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnI" Text="I" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnJ" Text="J" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnK" Text="K" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnL" Text="L" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnM" Text="M" runat="server" OnClick="ClickStartWithLink" /><br />
                    <asp:LinkButton id="btnN" Text="N" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnO" Text="O" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnP" Text="P" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnQ" Text="Q" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnR" Text="R" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnS" Text="S" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnT" Text="T" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnU" Text="U" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnV" Text="V" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnW" Text="W" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnX" Text="X" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnY" Text="Y" runat="server" OnClick="ClickStartWithLink" />
                    <asp:LinkButton id="btnZ" Text="Z" runat="server" OnClick="ClickStartWithLink" /><br /><br />
                <asp:GridView ID="gvName" ShowHeaderWhenEmpty="True"
                runat="server" 
                CssClass="clsFormLabelLeft"
                AutoGenerateColumns="False" 
                Caption="Names Similar to Members"
                OnRowCommand="gvName_RowCommand" 
                ShowFooter="True" 
                BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                GridLines="Vertical" Font-Size="X-Small" ForeColor="Black"> 
                <RowStyle BackColor="#F7F7DE" />
            <Columns> 
                <asp:TemplateField Visible="true" HeaderText="Name" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                    <ItemTemplate> 
                        <asp:LinkButton ID="lnkName" runat="server" CausesValidation="False" CommandName="Link" Text='<%# Bind("name") %>'></asp:LinkButton> 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Similar Names" Visible="true"  HeaderStyle-HorizontalAlign="Left" SortExpression="cnt"> 
                    <ItemTemplate> 
                        <asp:Label ID="lblCnt" runat="server" Text='<%# Bind("cnt") %>'></asp:Label> 
                    </ItemTemplate> 
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField> 
            </Columns> 
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView> 
                </td>
                <td valign="top" align="left">
                    Or, type last name and first name to search for similar names:<br />
        <br />Last Name:<br />
        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    <br />
        <br />
        Partial or Full First Name (can be blank):<br />
        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btnListAuthors" runat="server" Text="List Similar Names" 
            onclick="btnListAuthors_Click" />
                    <br />
        <br />
    <asp:GridView ID="gvClient" 
        runat="server" 
        CssClass="clsFormLabelLeft"
        Caption="Cancer Center Member"
        AutoGenerateColumns="False" 
        DataKeyNames="client_id"
        ShowFooter="True" 
        BackColor="White" BorderColor="#DEDFDE" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        GridLines="Vertical" ForeColor="Black"> 
        <RowStyle BackColor="#F7F7DE" />
    <Columns> 
        <asp:TemplateField HeaderText="ID" Visible="true"  HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
            <ItemTemplate> 
                <asp:Label ID="lblId" runat="server" Text='<%# Bind("client_id") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Last Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="last_name"> 
            <ItemTemplate> 
                <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("last_name") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="First Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="first_name"> 
            <ItemTemplate> 
                <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("first_name") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Initial" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="MI"> 
            <ItemTemplate> 
                <asp:Label ID="lblMI" runat="server" Text='<%# Bind("MI") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Start Date" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("start_date") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="End Date" Visible="true" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("end_date") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
    </Columns> 
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView> 
        <br />
    <asp:GridView ID="gvAuthor" 
        runat="server" 
        CssClass="clsFormLabelLeft"
        Caption="Publication Authors"
        AutoGenerateColumns="False" 
        OnRowCommand="gvAuthor_RowCommand"
        OnRowEditing="gvAuthor_RowEditing" 
        OnRowCancelingEdit="gvAuthor_RowCancelingEdit" 
        OnRowDataBound="gvAuthor_RowDataBound" 
        OnRowUpdating="gvAuthor_RowUpdating" 
        OnRowDeleting="gvAuthor_RowDeleting" 
        DataKeyNames="author_id"
        ShowFooter="True" 
        BackColor="White" BorderColor="#DEDFDE" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4" 
        GridLines="Vertical" ForeColor="Black"> 
        <RowStyle BackColor="#F7F7DE" />
    <Columns> 
        <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Left" Visible="false" SortExpression="author_id"> 
            <EditItemTemplate> 
                <asp:Label ID="lblId" runat="server" Text='<%# Bind("author_id") %>'></asp:Label>
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblId" runat="server" Text='<%# Bind("author_id") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Select" Visible="false">
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chxSelect" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="true" HeaderText="Pubs" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
            <ItemTemplate> 
                <asp:LinkButton ID="lnkLink" runat="server" CausesValidation="False" CommandName="Link"></asp:LinkButton> 
            </ItemTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Last Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="LastName"> 
            <ItemTemplate> 
                <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Fore Name" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="ForeName"> 
            <ItemTemplate> 
                <asp:Label ID="lblForeName" runat="server" Text='<%# Bind("ForeName") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Initials" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="Initials"> 
            <ItemTemplate> 
                <asp:Label ID="lblInitials" runat="server" Text='<%# Bind("Initials") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Member ID" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="client_id"> 
            <EditItemTemplate> 
                <asp:TextBox ID="txtClientId" Width="30px" runat="server" Text='<%# Bind("client_id") %>'></asp:TextBox> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblClientId" runat="server" Text='<%# Bind("client_id") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Member" Visible="true" HeaderStyle-HorizontalAlign="Left" SortExpression="confirm"> 
            <ItemTemplate> 
                <asp:Label ID="lblConfirm" runat="server" Text='<%# Bind("confirm") %>'></asp:Label> 
            </ItemTemplate> 
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:TemplateField> 
        <asp:TemplateField Visible="true" HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Save"></asp:LinkButton> 
                <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
                <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this author and all his/her publications?');" ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"></asp:LinkButton> 
            </ItemTemplate> 
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
        <asp:Button ID="ProcessSelectedAuthors" runat="server" 
            onclick="ProcessSelectedAuthors_Click" Text="Re-Process Selected Authors" 
                        Visible="False" /> 
        <br />
                    <asp:Label ID="lblDescription" runat="server" 
                        Text="From 'Edit', it will re-create formated author list, re-assign programs, and re-calculate programmatic information for publications with these selected authors. " 
                        Visible="False"></asp:Label>
        <br />
                </td>
            </tr>
            <tr>
                <td valign="top" width="25%">
                    &nbsp;</td>
                <td valign="top" align="left">
                    PMID:<asp:TextBox ID="txtPmid" runat="server"></asp:TextBox>
                    <asp:Button ID="btnProcessPmid" runat="server" Text="Process Test" 
                        onclick="btnProcessPmid_Click" />
                </td>
            </tr>
        </table>
    </div>
    <!--
    aspModalPopupExtender ID="mpext" run PopupControlID="pnlPopup" TargetControlID="pnlPopup" BackgroundCssClass="modalBackground" />
    -->
</asp:Content>

