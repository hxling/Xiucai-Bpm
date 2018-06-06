/* 帅特龙 项目甘特图 */
var JSGantt; if (!JSGantt) JSGantt = {}
var vTimeout = 0; var vBenchTime = new Date().getTime(); JSGantt.isIE = function () { if (typeof document.all != 'undefined') return true; else return false; }
JSGantt.TaskItem = function (pID, pName, pStart, pEnd, pColor, pLink, pMile, pRes, pComp, pGroup, pParent, pOpen, pDepend, pCaption) {
    var vID = pID; var vName = pName; var vStart = new Date(); var vEnd = new Date(); var vColor = pColor; var vLink = pLink; var vMile = pMile; var vRes = pRes; var vComp = pComp; var vGroup = pGroup; var vParent = pParent; var vOpen = pOpen; var vDepend = pDepend; var vCaption = pCaption; var vDuration = ''; var vLevel = 0; var vNumKid = 0; var vVisible = 1; var x1, y1, x2, y2; if (vGroup != 1) { vStart = JSGantt.parseDateStr(pStart, g.getDateInputFormat()); vEnd = JSGantt.parseDateStr(pEnd, g.getDateInputFormat()); }
    this.getID = function () { return vID; }
    this.getName = function () { return vName; }
    this.getStart = function () { return vStart; }
    this.getEnd = function () { return vEnd; }
    this.getColor = function () { return vColor; }
    this.getLink = function () { return vLink; }
    this.getMile = function () { return vMile; }
    this.getDepend = function () { if (vDepend) return vDepend; else return null; }
    this.getCaption = function () { if (vCaption) return vCaption; else return ''; }
    this.getResource = function () { if (vRes) return vRes; else return '&nbsp'; }
    this.getCompVal = function () { if (vComp) return vComp; else return 0; }
    this.getCompStr = function () { if (vComp) return vComp + '%'; else return ''; }
    this.getDuration = function (vFormat) {
        if (vMile) vDuration = '-'; else if (vFormat == 'hour') { tmpPer = Math.ceil((this.getEnd() - this.getStart()) / (60 * 60 * 1000)); if (tmpPer == 1) vDuration = '1 小时'; else vDuration = tmpPer + ' 小时'; } else if (vFormat == 'minute') { tmpPer = Math.ceil((this.getEnd() - this.getStart()) / (60 * 1000)); if (tmpPer == 1) vDuration = '1 分钟'; else vDuration = tmpPer + ' 分钟'; } else { tmpPer = Math.ceil((this.getEnd() - this.getStart()) / (24 * 60 * 60 * 1000) + 1); if (tmpPer == 1) vDuration = '1 天'; else vDuration = tmpPer + ' 天'; }
        return (vDuration)
    }
    this.getParent = function () { return vParent; }
    this.getGroup = function () { return vGroup; }
    this.getOpen = function () { return vOpen; }
    this.getLevel = function () { return vLevel; }
    this.getNumKids = function () { return vNumKid; }
    this.getStartX = function () { return x1; }
    this.getStartY = function () { return y1; }
    this.getEndX = function () { return x2; }
    this.getEndY = function () { return y2; }
    this.getVisible = function () { return vVisible; }
    this.setDepend = function (pDepend) { vDepend = pDepend; }
    this.setStart = function (pStart) { vStart = pStart; }
    this.setEnd = function (pEnd) { vEnd = pEnd; }
    this.setLevel = function (pLevel) { vLevel = pLevel; }
    this.setNumKid = function (pNumKid) { vNumKid = pNumKid; }
    this.setCompVal = function (pCompVal) { vComp = pCompVal; }
    this.setStartX = function (pX) { x1 = pX; }
    this.setStartY = function (pY) { y1 = pY; }
    this.setEndX = function (pX) { x2 = pX; }
    this.setEndY = function (pY) { y2 = pY; }
    this.setOpen = function (pOpen) { vOpen = pOpen; }
    this.setVisible = function (pVisible) { vVisible = pVisible; } 
}
JSGantt.GanttChart = function (pGanttVar, pDiv, pFormat, pProjectName) {
    var vGanttVar = pGanttVar; var vDiv = pDiv; var vFormat = pFormat; var vShowRes = 1; var vShowDur = 1; var vShowComp = 1; var vShowStartDate = 1; var vShowEndDate = 1; var vDateInputFormat = "yyyy-mm-dd"; var vDateDisplayFormat = "%F"; var vNumUnits = 0; var vCaptionType; var vDepId = 1; var vTaskList = new Array(); var vFormatArr = new Array("day", "week", "month", "quarter"); var vQuarterArr = new Array(1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4); var vMonthDaysArr = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31); var vMonthArr = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"); this.setFormatArr = function () {
        vFormatArr = new Array(); for (var i = 0; i < arguments.length; i++) { vFormatArr[i] = arguments[i]; }
        if (vFormatArr.length > 4) { vFormatArr.length = 4; } 
    }
    this.setShowRes = function (pShow) { vShowRes = pShow; }
    this.setShowDur = function (pShow) { vShowDur = pShow; }
    this.setShowComp = function (pShow) { vShowComp = pShow; }
    this.setShowStartDate = function (pShow) { vShowStartDate = pShow; }
    this.setShowEndDate = function (pShow) { vShowEndDate = pShow; }
    this.setDateInputFormat = function (pShow) { vDateInputFormat = pShow; }
    this.setDateDisplayFormat = function (pShow) { vDateDisplayFormat = pShow; }
    this.setCaptionType = function (pType) { vCaptionType = pType; }
    this.setFormat = function (pFormat) { vFormat = pFormat; this.Draw(); }
    this.getShowRes = function () { return vShowRes; }
    this.getShowDur = function () { return vShowDur; }
    this.getShowComp = function () { return vShowComp }
    this.getShowStartDate = function () { return vShowStartDate; }
    this.getShowEndDate = function () { return vShowEndDate; }
    this.getDateInputFormat = function () { return vDateInputFormat; }
    this.getDateDisplayFormat = function () { return vDateDisplayFormat; }
    this.getCaptionType = function () { return vCaptionType; }
    this.CalcTaskXY = function () { var vList = this.getList(); var vTaskDiv; var vParDiv; var vLeft, vTop, vHeight, vWidth; for (i = 0; i < vList.length; i++) { vID = vList[i].getID(); vTaskDiv = document.getElementById("taskbar_" + vID); vBarDiv = document.getElementById("bardiv_" + vID); vParDiv = document.getElementById("childgrid_" + vID); if (vBarDiv) { vList[i].setStartX(vBarDiv.offsetLeft); vList[i].setStartY(vParDiv.offsetTop + vBarDiv.offsetTop + 6); vList[i].setEndX(vBarDiv.offsetLeft + vBarDiv.offsetWidth); vList[i].setEndY(vParDiv.offsetTop + vBarDiv.offsetTop + 6); } } }
    this.AddTaskItem = function (value) { vTaskList.push(value); }
    this.getList = function () { return vTaskList; }
    this.clearDependencies = function () {
        var parent = document.getElementById('rightside'); var depLine; var vMaxId = vDepId; for (i = 1; i < vMaxId; i++) { depLine = document.getElementById("line" + i); if (depLine) { parent.removeChild(depLine); } }
        vDepId = 1;
    }
    this.sLine = function (x1, y1, x2, y2) { vLeft = Math.min(x1, x2); vTop = Math.min(y1, y2); vWid = Math.abs(x2 - x1) + 1; vHgt = Math.abs(y2 - y1) + 1; vDoc = document.getElementById('rightside'); var oDiv = document.createElement('div'); oDiv.id = "line" + vDepId++; oDiv.style.position = "absolute"; oDiv.style.margin = "0px"; oDiv.style.padding = "0px"; oDiv.style.overflow = "hidden"; oDiv.style.border = "0px"; oDiv.style.zIndex = 0; oDiv.style.backgroundColor = "red"; oDiv.style.left = vLeft + "px"; oDiv.style.top = vTop + "px"; oDiv.style.width = vWid + "px"; oDiv.style.height = vHgt + "px"; oDiv.style.visibility = "visible"; vDoc.appendChild(oDiv); }
    this.dLine = function (x1, y1, x2, y2) { var dx = x2 - x1; var dy = y2 - y1; var x = x1; var y = y1; var n = Math.max(Math.abs(dx), Math.abs(dy)); dx = dx / n; dy = dy / n; for (i = 0; i <= n; i++) { vx = Math.round(x); vy = Math.round(y); this.sLine(vx, vy, vx, vy); x += dx; y += dy; } }
    this.drawDependency = function (x1, y1, x2, y2) { if (x1 + 10 < x2) { this.sLine(x1, y1, x1 + 4, y1); this.sLine(x1 + 4, y1, x1 + 4, y2); this.sLine(x1 + 4, y2, x2, y2); this.dLine(x2, y2, x2 - 3, y2 - 3); this.dLine(x2, y2, x2 - 3, y2 + 3); this.dLine(x2 - 1, y2, x2 - 3, y2 - 2); this.dLine(x2 - 1, y2, x2 - 3, y2 + 2); } else { this.sLine(x1, y1, x1 + 4, y1); this.sLine(x1 + 4, y1, x1 + 4, y2 - 10); this.sLine(x1 + 4, y2 - 10, x2 - 8, y2 - 10); this.sLine(x2 - 8, y2 - 10, x2 - 8, y2); this.sLine(x2 - 8, y2, x2, y2); this.dLine(x2, y2, x2 - 3, y2 - 3); this.dLine(x2, y2, x2 - 3, y2 + 3); this.dLine(x2 - 1, y2, x2 - 3, y2 - 2); this.dLine(x2 - 1, y2, x2 - 3, y2 + 2); } }
    this.DrawDependencies = function () { this.CalcTaskXY(); this.clearDependencies(); var vList = this.getList(); for (var i = 0; i < vList.length; i++) { vDepend = vList[i].getDepend(); if (vDepend) { var vDependStr = vDepend + ''; var vDepList = vDependStr.split(','); var n = vDepList.length; for (var k = 0; k < n; k++) { var vTask = this.getArrayLocationByID(vDepList[k]); if (vList[vTask].getVisible() == 1) this.drawDependency(vList[vTask].getEndX(), vList[vTask].getEndY(), vList[i].getStartX() - 1, vList[i].getStartY()); } } } }
    this.getArrayLocationByID = function (pId) { var vList = this.getList(); for (var i = 0; i < vList.length; i++) { if (vList[i].getID() == pId) return i; } }
    this.Draw = function () {
        var vMaxDate = new Date(); var vMinDate = new Date(); var vTmpDate = new Date(); var vNxtDate = new Date(); var vCurrDate = new Date(); var vTaskLeft = 0; var vTaskRight = 0; var vWeekBack = ''; var vNumCols = 0; var vID = 0; var vMainTable = ""; var vLeftTable = ""; var vRightTable = ""; var vDateRowStr = ""; var vItemRowStr = ""; var vColWidth = 0; var vColUnit = 0; var vChartWidth = 0; var vNumDays = 0; var vDayWidth = 0; var vStr = ""; var vNameWidth = 220; var vStatusWidth = 70; var vLeftWidth = 15 + 220 + 70 + 70 + 70 + 70 + 70; var gantthtmlstrarr = []; if (vTaskList.length > 0) {
            JSGantt.processRows(vTaskList, 0, -1, 1, 1); vMinDate = JSGantt.getMinDate(vTaskList, vFormat); vMaxDate = JSGantt.getMaxDate(vTaskList, vFormat); vColWidth = 20; vColUnit = 1; vNumDays = $D(vMaxDate).diff(vMinDate, 'day', false) + 1; vNumUnits = vNumDays / vColUnit; vChartWidth = vNumUnits * (vColWidth + 1); vDayWidth = (vColWidth / vColUnit) + (1 / vColUnit); if (vShowRes != 1) vNameWidth += vStatusWidth; if (vShowDur != 1) vNameWidth += vStatusWidth; if (vShowComp != 1) vNameWidth += vStatusWidth; if (vShowStartDate != 1) vNameWidth += vStatusWidth; if (vShowEndDate != 1) vNameWidth += vStatusWidth; var topColspan = 1; if (vShowRes == 1) { topColspan++; }
            if (vShowDur == 1) { topColspan++; }
            if (vShowComp == 1) { topColspan++; }
            if (vShowStartDate == 1) { topColspan++; }
            if (vShowEndDate == 1) { topColspan++; }
            gantthtmlstrarr.push('<div region="west" split="true"  id=leftside><table cellSpacing=0 cellPadding=0 id="leftTable" class="leftTable" width=' + vLeftWidth + '><tbody><tr class="left-table-top"><td style="width: 15px;">&nbsp;</td><td colspan="' + topColspan + '" class="projectame">' + pProjectName + '</td></tr>'); gantthtmlstrarr.push('<tr class="caption"><td style="width: 15px; height: 20px">&nbsp;</td><td style="width: ' + vNameWidth + 'px; height: 20px">任务名称</td>'); if (vShowRes == 1)
                gantthtmlstrarr.push('  <td style="width: ' + vStatusWidth + 'px; height: 20px" align=center nowrap>负责人</td>'); if (vShowDur == 1)
                gantthtmlstrarr.push('  <td style="width: ' + vStatusWidth + 'px; height: 20px" align=center nowrap>总时间</td>'); if (vShowComp == 1)
                gantthtmlstrarr.push('  <td style="width: ' + vStatusWidth + 'px; height: 20px" align=center nowrap>完成度</td>'); if (vShowStartDate == 1)
                gantthtmlstrarr.push('  <td style="width: ' + vStatusWidth + 'px; height: 20px" align=center nowrap>开始日期</td>'); if (vShowEndDate == 1)
                gantthtmlstrarr.push('  <td style="width: ' + vStatusWidth + 'px; height: 20px" align=center nowrap>结束日期</td>'); gantthtmlstrarr.push('</tr>'); for (i = 0; i < vTaskList.length; i++) {
                var task = vTaskList[i]; if (vTaskList[i].getGroup()) { vBGColor = "f3f3f3"; vRowType = "group"; } else { vBGColor = "ffffff"; vRowType = "row"; }
                vID = task.getID(); vPID = task.getParent(); if (task.getVisible() == 0)
                    gantthtmlstrarr.push('<tr class="task" id=child_' + vID + ' bgcolor=#' + vBGColor + ' style="display:none" >'); else
                    gantthtmlstrarr.push('<tr class="task" vid="' + vID + '" pid="' + vPID + '" id=child_' + vID + ' bgcolor=#' + vBGColor + '>'); var isok = task.getCompVal() == 100 ? "√" : "&nbsp;"; gantthtmlstrarr.push('  <td class="gdatehead" style="width: 15px;" >' + isok + '</td><td class=gname style="width: ' + vNameWidth + 'px;" nowrap><NOBR><span style="color: #aaaaaa">'); for (j = 1; j < task.getLevel(); j++) { gantthtmlstrarr.push('&nbsp;&nbsp;'); }
                gantthtmlstrarr.push('</span>'); if (task.getGroup()) {
                    if (task.getOpen() == 1)
                        gantthtmlstrarr.push('<span id="group_' + vID + '" class="tree-expanded" onclick="JSGantt.folder(' + vID + ',' + vGanttVar + ');"></span>'); else
                        gantthtmlstrarr.push('<span id="group_' + vID + '" class="tree-collapsed" onclick="JSGantt.folder(' + vID + ',' + vGanttVar + ');"></span>');
                } else { gantthtmlstrarr.push('<span style="color: #000000; font-weight:bold; font-size: 12px;">&nbsp;&nbsp;</span>'); }
                gantthtmlstrarr.push('<span style="cursor:pointer"><a class="taskname" href="#bardiv_' + vID + '">' + task.getName() + '</a></span></td>'); if (vShowRes == 1)
                    gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center>' + task.getResource() + '</td>'); if (vShowDur == 1)
                    gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center>' + task.getDuration(vFormat) + '</td>'); if (vShowComp == 1)
                    gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center>' + task.getCompStr() + '</td>'); if (vShowStartDate == 1) {
                    if (!task.getGroup())
                        gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center>' + $D(task.getStart()).strftime(vDateDisplayFormat) + '</td>'); else
                        gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center></td>');
                }
                if (vShowEndDate == 1) {
                    if (!task.getGroup())
                        gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center>' + $D(task.getEnd()).strftime(vDateDisplayFormat) + '</td>'); else
                        gantthtmlstrarr.push('  <td class=gname style="width: 60px;" align=center></td>');
                }
                gantthtmlstrarr.push('</tr>');
            }
            gantthtmlstrarr.push('</tbody></table></div>'); gantthtmlstrarr.push('<div region="center"  id=rightside><table style="width: ' + vChartWidth + 'px;" cellSpacing=0 cellPadding=0 class="rightTable"><tbody><tr class="right-table-top">'); vTmpDate.setFullYear(vMinDate.getFullYear(), vMinDate.getMonth(), vMinDate.getDate()); vTmpDate.setHours(0); vTmpDate.setMinutes(0); while (Date.parse(vTmpDate) <= Date.parse(vMaxDate)) { gantthtmlstrarr.push('<td class=gdatehead align=center colspan=7>' + $D(vTmpDate).strftime('%m-%d') + ' / '); gantthtmlstrarr.push($D(vTmpDate).add(6, 'days').strftime('%F') + '</td>'); vTmpDate.setDate(vTmpDate.getDate() + 1); }
            gantthtmlstrarr.push('</tr><tr class="right-date">'); vTmpDate.setFullYear(vMinDate.getFullYear(), vMinDate.getMonth(), vMinDate.getDate()); while (Date.parse(vTmpDate) <= Date.parse(vMaxDate)) {
                if ($D(vCurrDate).strftime('%F') == $D(vTmpDate).strftime('%F')) { vWeekdayColor = "ccccff"; vWeekendColor = "9999ff"; vWeekdayGColor = "bbbbff"; vWeekendGColor = "8888ff"; } else { vWeekdayColor = "ffffff"; vWeekendColor = "cfcfcf"; vWeekdayGColor = "f3f3f3"; vWeekendGColor = "c3c3c3"; }
                if (vTmpDate.getDay() % 6 == 0) { var weekLeft = Math.ceil((Date.parse(vTmpDate) - Date.parse(vMinDate)) / (24 * 60 * 60 * 1000)) * 21 - 1; gantthtmlstrarr.push('<td class="gheadwkend"  bgcolor=#' + vWeekendColor + ' align=center><div style="width: ' + vColWidth + 'px">' + vTmpDate.getDate() + '</div></td>'); vWeekBack += '<div class="weekbg" style="left:' + weekLeft + 'px;">&nbsp;</div>'; } else { gantthtmlstrarr.push('<td class="ghead" bgcolor=#' + vWeekdayColor + ' align=center><div style="width: ' + vColWidth + 'px">' + vTmpDate.getDate() + '</div></td>'); }
                vTmpDate.setDate(vTmpDate.getDate() + 1);
            }
            gantthtmlstrarr.push("</tr></tbody></table>"); for (i = 0; i < vTaskList.length; i++) {
                vTmpDate.setFullYear(vMinDate.getFullYear(), vMinDate.getMonth(), vMinDate.getDate()); vTaskStart = vTaskList[i].getStart(); vTaskEnd = vTaskList[i].getEnd(); vNumCols = 0; vID = vTaskList[i].getID(); vNumUnits = (vTaskEnd - vTaskStart) / (24 * 60 * 60 * 1000) + 1; if (vTaskList[i].getVisible() == 0)
                    gantthtmlstrarr.push('<div id=childgrid_' + vID + ' style="position:relative; display:none;">'); else
                    gantthtmlstrarr.push('<div id=childgrid_' + vID + ' style="position:relative">'); if (vTaskList[i].getMile()) {
                    gantthtmlstrarr.push('<div id=childrow_' + vID + ' class="rightTable-TaskItem" style="background:#f3f3f3; width: ' + vChartWidth + 'px;" >' + vItemRowStr + '</div>'); vDateRowStr = $D(vTaskStart).strftime(vDateDisplayFormat); vTaskLeft = $D(vTaskStart).diff(vMinDate, 'day', false) + 1; vTaskLeft = vTaskLeft * vDayWidth; var keyline = drawKeyLine(vID, vMinDate, vDayWidth); gantthtmlstrarr.push(keyline); vTaskRight = 1; gantthtmlstrarr.push(vWeekBack + '<div id=bardiv_' + vID + ' style="position:absolute; top:0px; left:' + vTaskLeft + 'px; height: 18px; width:160px; overflow:hidden;">'); gantthtmlstrarr.push('<div id=taskbar_' + vID + ' title="' + vTaskList[i].getName() + ': ' + vDateRowStr + '" style="height: 16px; width:12px; overflow:hidden; cursor: pointer;" >'); if (vTaskList[i].getCompVal() < 100)
                        gantthtmlstrarr.push('&loz;</div>'); else
                        gantthtmlstrarr.push('&diams;</div>'); if (g.getCaptionType()) {
                        vCaptionStr = ''; switch (g.getCaptionType()) { case 'Caption': vCaptionStr = vTaskList[i].getCaption(); break; case 'Resource': vCaptionStr = vTaskList[i].getResource(); break; case 'Duration': vCaptionStr = vTaskList[i].getDuration(vFormat); break; case 'Complete': vCaptionStr = vTaskList[i].getCompStr(); break; }
                        gantthtmlstrarr.push('<div style="font-size:12px; position:absolute; top:2px; width:120px; left:12px">' + vCaptionStr + '</div>');
                    }
                    gantthtmlstrarr.push('</div>');
                } else {
                    vDateRowStr = $D(vTaskStart).strftime(vDateDisplayFormat) + ' - ' + $D(vTaskEnd).strftime(vDateDisplayFormat); vTaskRight = (Date.parse(vTaskList[i].getEnd()) - Date.parse(vTaskList[i].getStart())) / (24 * 60 * 60 * 1000) + 1 / vColUnit; vTaskLeft = $D(vTaskStart).diff(vMinDate, 'day', false) + 1; vTaskLeft = vTaskLeft * vDayWidth; var tTime = new Date(); tTime.setTime(Date.parse(vTaskStart)); if (tTime.getMinutes() > 29) vTaskLeft += .5
                    var keyline = drawKeyLine(vID, vMinDate, vDayWidth); gantthtmlstrarr.push(keyline); if (vTaskList[i].getGroup()) { gantthtmlstrarr.push('<div id=childrow_' + vID + ' class="rightTable-TaskItem groupbg" style="width: ' + vChartWidth + 'px;" >' + vItemRowStr + '</div>'); gantthtmlstrarr.push(vWeekBack); gantthtmlstrarr.push('<div id=bardiv_' + vID + ' style="position:absolute; top:5px; left:' + vTaskLeft + 'px; height: 7px; width:' + Math.ceil((vTaskRight) * (vDayWidth) - 1) + 'px">'); } else { gantthtmlstrarr.push('<div id=childrow_' + vID + ' class="rightTable-TaskItem" style="width: ' + vChartWidth + 'px;" >' + vItemRowStr + '</div>'); gantthtmlstrarr.push(vWeekBack + '<div id=bardiv_' + vID + ' style="position:absolute; top:4px; left:' + vTaskLeft + 'px; height:18px; width:' + Math.ceil((vTaskRight) * (vDayWidth) - 1) + 'px">' + '<div id=taskbar_' + vID + ' title="' + vTaskList[i].getName() + ': ' + vDateRowStr + '" class=gtask style="background-color:#' + vTaskList[i].getColor() + '; height: 13px; width:' + Math.ceil((vTaskRight) * (vDayWidth) - 1) + 'px; cursor: pointer;opacity:0.9;" ' + '>' + '<div class=gcomplete style="Z-INDEX: -4; float:left; background-color:black; height:5px; overflow: auto; margin-top:4px; filter: alpha(opacity=40); opacity:0.4; width:' + vTaskList[i].getCompStr() + '; overflow:hidden">' + '</div>' + '</div>'); }
                    if (g.getCaptionType()) {
                        vCaptionStr = ''; switch (g.getCaptionType()) { case 'Caption': vCaptionStr = vTaskList[i].getCaption(); break; case 'Resource': vCaptionStr = vTaskList[i].getResource(); break; case 'Duration': vCaptionStr = vTaskList[i].getDuration(vFormat); break; case 'Complete': vCaptionStr = vTaskList[i].getCompStr(); break; }
                        if (!vTaskList[i].getGroup())
                            gantthtmlstrarr.push('<div style="font-size:12px; position:absolute; top:-3px; width:120px; left:' + (Math.ceil((vTaskRight) * (vDayWidth) - 1) + 6) + 'px">' + vCaptionStr + '</div>');
                    }
                    gantthtmlstrarr.push('</div>');
                }
                gantthtmlstrarr.push('</div>');
            }
            gantthtmlstrarr.push('</div>'); vDiv.innerHTML = gantthtmlstrarr.join(' ');
        } 
    } 
}
JSGantt.processRows = function (pList, pID, pRow, pLevel, pOpen) {
    var vMinDate = new Date(); var vMaxDate = new Date(); var vMinSet = 0; var vMaxSet = 0; var vList = pList; var vLevel = pLevel; var i = 0; var vNumKid = 0; var vCompSum = 0; var vVisible = pOpen; for (i = 0; i < pList.length; i++) {
        if (pList[i].getParent() == pID) {
            vVisible = pOpen; pList[i].setVisible(vVisible); if (vVisible == 1 && pList[i].getOpen() == 0) vVisible = 0; pList[i].setLevel(vLevel); vNumKid++; if (pList[i].getGroup() == 1) { JSGantt.processRows(vList, pList[i].getID(), i, vLevel + 1, vVisible); }
            if (vMinSet == 0 || pList[i].getStart() < vMinDate) { vMinDate = pList[i].getStart(); vMinSet = 1; }
            if (vMaxSet == 0 || pList[i].getEnd() > vMaxDate) { vMaxDate = pList[i].getEnd(); vMaxSet = 1; }
            vCompSum += pList[i].getCompVal();
        } 
    }
    if (pRow >= 0) { pList[pRow].setStart(vMinDate); pList[pRow].setEnd(vMaxDate); pList[pRow].setNumKid(vNumKid); pList[pRow].setCompVal(Math.ceil(vCompSum / vNumKid)); } 
}
JSGantt.getMinDate = function getMinDate(pList, pFormat) {
    var vDate = new Date(); var _tmpd = null; for (i = 0; i < pList.length; i++) {
        _tmpd = pList[i].getStart(); if (Date.parse(_tmpd) < Date.parse(vDate))
            vDate.setFullYear(_tmpd.getFullYear(), _tmpd.getMonth(), _tmpd.getDate());
    }
    if (pFormat == 'minute') { vDate.setHours(0); vDate.setMinutes(0); } else if (pFormat == 'hour') { vDate.setHours(0); vDate.setMinutes(0); }
    else if (pFormat == 'day') { vDate.setDate(vDate.getDate() - 1); while (vDate.getDay() % 7 > 0) { vDate.setDate(vDate.getDate() - 1); } } else if (pFormat == 'week') { vDate.setDate(vDate.getDate() - 7); while (vDate.getDay() % 7 > 0) { vDate.setDate(vDate.getDate() - 1); } } else if (pFormat == 'month') { while (vDate.getDate() > 1) { vDate.setDate(vDate.getDate() - 1); } } else if (pFormat == 'quarter') { if (vDate.getMonth() == 0 || vDate.getMonth() == 1 || vDate.getMonth() == 2) vDate.setFullYear(vDate.getFullYear(), 0, 1); else if (vDate.getMonth() == 3 || vDate.getMonth() == 4 || vDate.getMonth() == 5) vDate.setFullYear(vDate.getFullYear(), 3, 1); else if (vDate.getMonth() == 6 || vDate.getMonth() == 7 || vDate.getMonth() == 8) vDate.setFullYear(vDate.getFullYear(), 6, 1); else if (vDate.getMonth() == 9 || vDate.getMonth() == 10 || vDate.getMonth() == 11) vDate.setFullYear(vDate.getFullYear(), 9, 1); }
    return (vDate);
}
JSGantt.getMaxDate = function (pList, pFormat) {
    var vDate = new Date(); var _tmp = null; for (i = 0; i < pList.length; i++) { _tmp = pList[i].getEnd(); if (Date.parse(_tmp) > Date.parse(vDate)) { vDate.setTime(Date.parse(_tmp)); } }
    if (pFormat == 'minute') { vDate.setHours(vDate.getHours() + 1); vDate.setMinutes(59); }
    if (pFormat == 'hour') { vDate.setHours(vDate.getHours() + 2); }
    if (pFormat == 'day') { vDate.setDate(vDate.getDate() + 1); while (vDate.getDay() % 6 > 0) { vDate.setDate(vDate.getDate() + 1); } }
    if (pFormat == 'week') { vDate.setDate(vDate.getDate() + 11); while (vDate.getDay() % 6 > 0) { vDate.setDate(vDate.getDate() + 1); } }
    if (pFormat == 'month') {
        while (vDate.getDay() > 1) { vDate.setDate(vDate.getDate() + 1); }
        vDate.setDate(vDate.getDate() - 1);
    }
    if (pFormat == 'quarter') { if (vDate.getMonth() == 0 || vDate.getMonth() == 1 || vDate.getMonth() == 2) vDate.setFullYear(vDate.getFullYear(), 2, 31); else if (vDate.getMonth() == 3 || vDate.getMonth() == 4 || vDate.getMonth() == 5) vDate.setFullYear(vDate.getFullYear(), 5, 30); else if (vDate.getMonth() == 6 || vDate.getMonth() == 7 || vDate.getMonth() == 8) vDate.setFullYear(vDate.getFullYear(), 8, 30); else if (vDate.getMonth() == 9 || vDate.getMonth() == 10 || vDate.getMonth() == 11) vDate.setFullYear(vDate.getFullYear(), 11, 31); }
    return (vDate);
}
JSGantt.findObj = function (theObj, theDoc) {
    var p, i, foundObj; if (!theDoc) theDoc = document; if ((p = theObj.indexOf("?")) > 0 && parent.frames.length) { theDoc = parent.frames[theObj.substring(p + 1)].document; theObj = theObj.substring(0, p); }
    if (!(foundObj = theDoc[theObj]) && theDoc.all) foundObj = theDoc.all[theObj]; for (i = 0; !foundObj && i < theDoc.forms.length; i++) foundObj = theDoc.forms[i][theObj]; for (i = 0; !foundObj && theDoc.layers && i < theDoc.layers.length; i++) foundObj = JSGantt.findObj(theObj, theDoc.layers[i].document); if (!foundObj && document.getElementById) foundObj = document.getElementById(theObj); return foundObj;
}
JSGantt.folder = function (pID, ganttObj) {
    var vList = ganttObj.getList(); for (i = 0; i < vList.length; i++) { if (vList[i].getID() == pID) { if (vList[i].getOpen() == 1) { vList[i].setOpen(0); JSGantt.hide(pID, ganttObj); } else { vList[i].setOpen(1); JSGantt.show(pID, ganttObj); } } }
    return false;
}
JSGantt.hide = function (pID, ganttObj) { var vList = ganttObj.getList(); $('#group_' + pID).attr('class', "tree-collapsed"); $('#leftTable tr.task').each(function (i, n) { var vid = $(n).attr('vid'); var pid = $(n).attr('pid'); if (pid == pID) { $(n).hide(); $('#childgrid_' + vid).hide(); vList[i].setVisible(0); if (vList[i].getGroup() == 1) JSGantt.hide(vid, ganttObj); } }); }
JSGantt.show = function (pID, ganttObj) { var vList = ganttObj.getList(); $('#group_' + pID).attr('class', "tree-expanded"); $('#leftTable tr.task').each(function (i, n) { var vid = $(n).attr('vid'); var pid = $(n).attr('pid'); if (pid == pID) { $(n).show(); $('#childgrid_' + vid).show(); vList[i].setVisible(1); if (vList[i].getGroup() == 1) JSGantt.show(vid, ganttObj); } }); }
JSGantt.taskLink = function (pRef, pWidth, pHeight) { if (pWidth) vWidth = pWidth; else vWidth = 400; if (pHeight) vHeight = pHeight; else vHeight = 400; var OpenWindow = window.open(pRef, "newwin", "height=" + vHeight + ",width=" + vWidth); }
JSGantt.parseDateStr = function (pDateStr, pFormatStr) {
    var vDate = new Date(); vDate.setTime(Date.parse(pDateStr)); switch (pFormatStr) { case 'mm/dd/yyyy': var vDateParts = pDateStr.split('/'); vDate.setFullYear(parseInt(vDateParts[2], 10), parseInt(vDateParts[0], 10) - 1, parseInt(vDateParts[1], 10)); break; case 'dd/mm/yyyy': var vDateParts = pDateStr.split('/'); vDate.setFullYear(parseInt(vDateParts[2], 10), parseInt(vDateParts[1], 10) - 1, parseInt(vDateParts[0], 10)); break; case 'yyyy-mm-dd': var vDateParts = pDateStr.split('-'); vDate.setFullYear(parseInt(vDateParts[0], 10), parseInt(vDateParts[1], 10) - 1, parseInt(vDateParts[2], 10)); break; }
    return (vDate);
}
function drawine(kd, vmindate, colDaywidth) {
    var keyLineDiv = ''; var keyLineLeft = 0; for (var i = 0; i < kd.length; i++) { keyLineLeft = ($D(kd[i].keydate).diff(vmindate, 'days', false) + 1) * colDaywidth; keyLineDiv += '<div tip="关键时间点：\n' + kd[i].remark + '" style="left:' + keyLineLeft + 'px;" class="keyline2"></div>'; }
    return keyLineDiv;
}
function drawKeyLine(proid, vmindate, colDaywidth) {
   
    if (proid.indexOf('_') > -1)
        proid = proid.substr(0, proid.length - 1);
    var keynodes; $.each(keydate, function (i, n) {
        if (n.projectid == proid)
            keynodes = n.keynodes;
    });
     if (keynodes.length > 0)
        return drawine(keynodes, vmindate, colDaywidth); else
        return "";
}