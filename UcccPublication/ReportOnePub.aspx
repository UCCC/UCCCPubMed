<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportOnePub.aspx.cs" Inherits="ReportOnePub" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            text-align: center;
        }
        .style3
        {
            text-align: left;
        }
        .style4
        {
            font-weight: bold;
            text-decoration: underline;
        }
        .style5
        {
            font-family: Arial;
        }
        .style6
        {
            font-family: Arial;
            font-size: xx-small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="style1">
            <tr>
                <td class="style2">
                    <div class="style2">
                    </div>
                    <asp:placeholder id="topPlaceholder" runat="server"/>
                    <div class="style3">
                        <asp:Label ID="lblExclusiveNote" runat="server"></asp:Label>
                        <br /><br />
                    </div>
                </td>
            </tr>
        </table>
    <asp:placeholder id="GridViewPlaceHolder"
        runat="server"/>


    </div>
    <p class="style6">
        * = Inter-programmatic publication; + = Intra-programmatic publication; *+ = 
        Inter-/Intra-programmatic publication<br />
        Member names are highlighted and their respective programs are indicated in 
        parentheses.<br />
        Program abbreviations: CB = Cancer Cell Biology; PC = Cancer Prevention and 
        Control; DT = Developmental Therapeutics; HRM = Hormone Related Malignancies; MO 
        = Molecular Oncology; LHN = Lung, Head &amp; Neck Cancers</p>
    </form>
    </body>
</html>
