<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>CPU Scheduling</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.2/css/all.css" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:Timer ID="Timer1" runat="server" Interval="4500" OnTick="Timer1_Tick"></asp:Timer>

        <div class="jumbotron jumbotron-fluid">
            <div class="container">
                <h1 class="display-4"><i class="fas fa-microchip"></i>&nbsp;CPU Scheduling Simulation</h1>
                <p class="lead"><i class="fas fa-code"></i>&nbsp;Prepared by Alperen Köylü (100042970) for Operating Systems Homework Assignment.</p>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>

                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />

            </Triggers>
            <ContentTemplate>
                <div class="container">
                    <asp:Panel ID="pnlAlert" CssClass="row" runat="server">
                        <div class="alert alert-success container-fluid" role="alert">
                            <h4 class="alert-heading"><i class="fas fa-check-circle"></i>&nbsp;CPU Finished The Sequence , Now It's Idle </h4>
                            <p>
                                Before the sequence Queue A had a <b>
                                    <asp:Literal ID="ltrQAPick" runat="server"></asp:Literal></b> of pick ratio,
                        meanwhile Queue B had a <b>
                            <asp:Literal ID="ltrQBPick" runat="server"></asp:Literal></b> of pick ratio.
                            </p>
                            <p>
                                In past sequence Queue A had length of <b>
                                    <asp:Literal ID="ltrQALen" runat="server"></asp:Literal></b> processes (AVG Length of Queue A = <asp:Literal ID="totalA" runat="server"></asp:Literal>),
                        Queue B had <b>
                            <asp:Literal ID="ltrQBLen" runat="server"></asp:Literal></b> (AVG Length of Queue B = <asp:Literal ID="totalB" runat="server"></asp:Literal>).
                            </p>
                            <hr />
                            <p class="mb-0">
                                <i class="fas fa-asterisk text-danger"></i>&nbsp;After the calculations, the new pick ratio is <b>
                                    <asp:Literal ID="ltrNewQAPick" runat="server"></asp:Literal></b> for Queue A,
                        <b>
                            <asp:Literal ID="ltrNewQBPick" runat="server"></asp:Literal></b> for Queue B.
                            </p>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-8">
                            <div class="row">
                                <asp:LinkButton ID="LinkButton1" CssClass="btn btn-danger btn-lg btn-block" runat="server" OnClick="LinkButton1_Click">
                            <i class="fas fa-play"></i>&nbsp;Simulate
                                </asp:LinkButton>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="alert alert-warning col-sm text-center mr-1">
                                    <h1>Queue A (FCFS)</h1>
                                    <table class="table table-striped table-borderless table-hover">
                                        <thead class="thead-dark">
                                            <tr>
                                                <th scope="col">Process</th>
                                                <th scope="col">Arrival Time</th>
                                                <th scope="col">Burst Time</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr></tr>
                                            <asp:Repeater ID="rptQueueA" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%#Eval("name")%></td>
                                                        <td><%#Eval("arrival_time")%></td>
                                                        <td><%#Eval("burst_time")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="alert alert-info col-sm text-center ml-1">
                                    <h1>Queue B (SJF)</h1>
                                    <table class="table table-striped table-borderless table-hover">
                                        <thead class="thead-dark">
                                            <tr>
                                                <th scope="col">Process</th>
                                                <th scope="col">Arrival Time</th>
                                                <th scope="col">Burst Time</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptQueueB" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%#Eval("name")%></td>
                                                        <td><%#Eval("arrival_time")%></td>
                                                        <td><%#Eval("burst_time")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="row">
                                <div class="col-sm alert alert-primary ml-2">

                                    <asp:Panel ID="simulating" runat="server">
                                        <div class="alert alert-secondary text-center" role="alert">
                                            <h2>SIMULATING <i class="fas fa-circle-notch fa-spin"></i></h2>
                                        </div>
                                    </asp:Panel>

                                    <div class="alert alert-secondary" role="alert">
                                        <asp:Chart ID="Chart1" runat="server">
                                            <Titles>
                                                <asp:Title Name="Title1" Text="Average Waiting Time"></asp:Title>
                                            </Titles>
                                            <Series>
                                                <asp:Series Name="Series1"></asp:Series>
                                            </Series>
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </div>
                                    <div class="alert alert-secondary" role="alert">
                                        <asp:Literal ID="ltrLOG" runat="server">

                                        </asp:Literal>
                                    </div>
                                    <%--
                                    <div class="alert alert-secondary" role="alert">
                                        <h4 class="alert-heading">#2 Sequence Completed</h4>
                                        <hr />
                                        <p>Queue Pick Ratios : A = 70%, B = 30%</p>
                                        <p>Queue Lengths : A = 11, B = 22</p>
                                        <p>Start Time : 0 ms,</p>
                                        <p>Finsih Time : 12232 ms,</p>
                                        <p class="mb-0">Elapsed Time : 12232 ms</p>
                                    </div>
                                    --%>
                                </div>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
</body>
</html>
