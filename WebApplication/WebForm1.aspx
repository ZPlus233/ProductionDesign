<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
        <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <div style="background: #6699FF; margin: 2px;height:70px;">
        <label style="font-size: 40px;padding-left: 15px;color: white;margin-top:5px;">智能纺纱排产系统</label>
    </div>
    <form id="form1" runat="server" style="margin-top:4px">
        <div class="container" style="width: 100%;">
            <div class="row" >
                <div class="col-lg-2" >
                    <div style="width:22.5em;height:50px;background-color:#6699FF;">
                        <label style="width:inherit;color:white;text-align: center;font-size: 2em;margin: 0.2em;">排产</label>
                    </div>
                    <asp:Button ID="Button7" runat="server" Text="订单配置"  Height="60px" OnClick="Button7_Click" style="margin:0.1em;font-size:1.5em;" class="btn btn-info btn-lg" Width="14.9em" /><br/>
                    <asp:Button ID="Button11" runat="server" Text="当前生产状态配置"  style="margin:0.1em;font-size:1.5em;" Height="60px" OnClick="Button11_Click"  class="btn btn-info btn-lg" Width="14.9em" /><br/>
                    <asp:Button ID="Button9" runat="server" Text="开始排产"  Height="60px" OnClick="Button9_Click" style="margin:0.1em;font-size:1.5em;" class="btn btn-info btn-lg" Width="14.9em" /><br/>
                    <div style="width:22.5em;height:50px;background-color:#6699FF;">
                        <label  id="labelParameterSet" style="width:inherit;color:white;text-align: center;font-size: 2em;margin: 0.2em;">参数设置</label>
                    </div>
                    <asp:Button ID="Button3" runat="server" Text="1各工序制成率_设备运转率" style="margin:0.1em;font-size:0.93em;" Height="60px" OnClick="Button3_Click" class="btn btn-info btn-lg" Width="24em" /><br/>
                    <asp:Button ID="Button4" runat="server" Text="2机器生产参数配置" style="margin:0.1em;font-size:0.93em;" Height="60px" OnClick="Button4_Click" class="btn btn-info btn-lg" Width="24em" /><br/>
                    <asp:Button ID="Button5" runat="server" Text="3产品干湿重g_m配置" style="margin:0.1em;font-size:0.93em;" Height="60px" OnClick="Button5_Click"  class="btn btn-info btn-lg" Width="24em" /><br />
                    <asp:Button ID="Button6" runat="server" Text="4各工序切换产品所需时间"  style="margin:0.1em;font-size:0.93em;" Height="60px" OnClick="Button6_Click"  class="btn btn-info btn-lg" Width="24em" /><br/>
                </div>
                <div class="col-lg-8 col-lg-offset-1">
                     <div id ="readExcel" style="margin-top:10px;">
                        <asp:Table ID="Table3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px" HorizontalAlign="Justify" >
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="Button13" runat="server" class="btn btn-info" style="margin-right: 5em;width: 10em;" Text="下载模板" OnClick="Button13_Click"/>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:FileUpload ID="FileUpload1"  class="" Text="请选择上传文件" onchange="TextFileUpload1.value=this.value"  runat="server" style="display:none" />
                                <input id="TextFileUpload1" type="text"  value="...上传文件" disabled="disabled" />
                                <input id="ButtonFileUpload1" type="button" value="选择上传文件" class="btn btn-info" style="margin-right: 5em;width: 10em;"  onclick="FileUpload1.click()" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="Button12" runat="server" class="btn btn-info" style="margin-right: 5em;width: 10em;" Text="确定" OnClick="Button12_Click"/>
                            </asp:TableCell>
                        </asp:TableRow>
                        </asp:Table>
                        <asp:Table ID="Table4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Outset" BorderWidth="1px" HorizontalAlign="Justify" ></asp:Table>
                    </div>
                    <div id="setting">
                        <asp:Table ID="Table2" runat="server" BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px" HorizontalAlign="Justify" style="margin-top: 0px" >
                        <asp:TableRow style="height: 3em;">
                            <asp:TableCell style="width: 7em;">
                                <asp:Label ID="Label1" runat="server" Text="迭代次数"></asp:Label>
                             </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="M" runat="server" TextMode="Number" placeholder="10"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow style="height: 3em;">
                             <asp:TableCell style="width: 7em;">
                                <asp:Label ID="Label2" runat="server" Text="种群规模"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="N" runat="server" TextMode="Number" placeholder="40"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow style="height: 3em;">
                        <asp:TableCell style="width: 7em;">
                            <asp:Label ID="Label3" runat="server" Text="变异概率"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="Pm" runat="server" TextMode="Number" placeholder="0.5"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell></asp:TableCell>
                        <asp:TableCell HorizontalAlign="Right">
                        <asp:Button ID="Button10" runat="server" Text="提交" class="btn btn-info" style="margin-right: 5em;width: 10em;" OnClick="Button10_Click"/></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                   <asp:Table ID="Table5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="None" BorderWidth="1px" HorizontalAlign="Justify" >
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="Button15" runat="server" class="btn btn-info" style="margin-right: 5em;width: 10em;" Text="点击开始排产" OnClick="Button15_Click"/>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="Button14" runat="server" class="btn btn-info" style="margin-right: 5em;width: 10em;" Text="下载排产结果" OnClick="Button14_Click"/>
                        </asp:TableCell>
                    </asp:TableRow>  
                </asp:Table>
                <%--<asp:Table ID="Table6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Outset" BorderWidth="1px" HorizontalAlign="Justify" >
                </asp:Table>--%>
                <asp:Label ID="resultShow" runat="server" Text="Label"></asp:Label>
                <div id="getForm">
                    <asp:Table ID="Table1" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Outset" BorderWidth="1px" HorizontalAlign="Justify" Width="80%" ></asp:Table>
                </div>
            </div>
        </div>
        </div>
        </div>
    </form>
</body>
</html>
