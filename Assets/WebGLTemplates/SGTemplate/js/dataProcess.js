/*
 * @description 是否显示测试用的FPS
 */
function jsIsShowFPS() {
    return false;
}

/*
 * @description 称判断是否为IOS浏览器
 */
function isIOSBrowser() {
    var UA = navigator.userAgent;
    if (UA.match(/iPad/) || UA.match(/iPhone/) || UA.match(/iPod/)) {
        return true;
    } else {
        return false;
    }
}

/**
 * @description 加载视图（由Unity调用）
 * @param {string} data JSON格式数据
 * 对象格式 class DataInfo{ stirng ObjectID, int Type, string DataName, string[] DataDetails }
 */
function jsLoadView(data) {
    // 解析成对象
    let objData = JSON.parse(data);
    var dataType;
    switch (objData.Type) {
        case 0: dataType = "Video"; break;
        case 1: dataType = "Image"; break;
        case 2: dataType = "ImageText"; break;
        case 3: dataType = "ComicStrip"; break;
        case 4: dataType = "Game"; break;
        case 5: dataType = "Url"; break;
        default: break;
    }

    // 不同类型加载不同的HTML文件
    var url = dataType + ".html";

    // 文件内容加载到divView
    var req = false;
    if (window.XMLHttpRequest) {// Safari, Firefox, 及其他非微软浏览器
        try {
            req = new XMLHttpRequest();
        } catch (e) {
            req = false;
        }
    } else if (window.ActiveXObject) {
        try {
            req = new ActiveXObject("Msxml2.XMLHTTP");// For Internet Explorer on Windows
        } catch (e) {
            try {
                req = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                req = false;
            }
        }
    }
    var element = document.getElementById("divView");
    if (!element) {
        alert("无法找到id为" + id + "的div 或 span 标签。");
        return;
    }
    if (req) {
        req.open('GET', url, false);    // 同步请求，等待收到全部内容
        req.send(null);
        element.innerHTML = req.responseText;
        // $("#divView").html(req.responseText);

        // 自动加载并根据类型加载对应的资源进行显示
        if (dataType == "Video") {
            jsLoadVideo(objData);
        }
        else if (dataType == "Image") {
            jsLoadImage(objData);
        }
        else if (dataType == "Url") {
            jsLoadUrl(objData.DataName);
        }
        else if (dataType == "ComicStrip") {
            jsLoadComicStrip(objData.DataName);
        }
        else if (dataType == "ImageText") {
            // 测试写法，不代表最终写法。这里只传递一个参数，图文应该有图片和音频。
            jsLoadImageText(objData.DataName);
        }
    } else {
        element.innerHTML =
            "对不起，你的浏览器不支持" + "XMLHTTPRequest 对象。这个网页的显示要求" +
            "Internet Explorer 5 以上版本, " + "或 Firefox 或 Safari 浏览器，也可能会有其他可兼容的浏览器存在。";
    }
}

/**
 * @description 移动时关闭弹窗并清空divView内容
 */
function jsHideView() {
    $('#modal').addClass('center-modal');
    $('#modal').removeClass('maxmize-modal');
    modal.style.display = "none";
    //$('#modal').fadeOut();    // 会有标题在

    var element = document.getElementById("divView");
    if (!element) {
        alert("无法找到id为" + id + "的div 或 span 标签。");
        return;
    }
    else {
        element.innerHTML = "";
    }
}

/**
 * @description 加载视频
 * @param {DataInfo} datainfo 数据信息
 */
function jsLoadVideo(videoName,videoTitle) {
    var divVideoPlayer = document.querySelector("#divVideoPlayer");
    var btnClose = document.querySelector("#btnClose");
    //var vdMain = document.querySelector("#vdPlayer");
    //var vSource = document.querySelector("#vdSource");
    var vdTitle = document.querySelector("#vdTitle");
    //vSource.src = "StreamingAssets/Video/" + videoName;
    //vdMain.load();
    divVideoPlayer.style.display = "block";
    btnClose.style.display = "block";
    vdTitle.innerHTML = videoTitle;
}

/**
 * @description 加载图像
 * @param {DataInfo} datainfo 数据信息
 */
function jsLoadImage(datainfo) {
    var imgMain = document.querySelector("#imgMain");
    imgMain.src = "Content/Images/" + datainfo.DataName;

    // 如果包含音频，则显示音频播放按钮
    if (datainfo.DataDetails.length > 1) {
        var auSource = document.querySelector("#auSource");
        auSource.src = "Content/Audios/" + datainfo.DataDetails[1];
        var btnPlay = document.querySelector("#btnPlay");
        btnPlay.style.display = "block";
    }

    $('#modal').fadeIn(500);
}

/**
 * @description 图文
 * @param {string} audioName 音频名称
 * @param {string} imageName 图片名称
 */
function jsLoadImageText(audioName, imageName) {
    var adMain = document.querySelector("#adMain");
    var aSource = document.querySelector("#aSource");
    aSource.src = "Content/Audios/" + audioName;
    // 这里图片应该动态添加，目前是在imageText.html写死
    $('#imgText').attr('src', 'Content/Images/' + imageName);
    adMain.load();
    $('#modal').fadeIn(500);
    setTimeout(function () {
        if (!isIOSBrowser()) {
            adMain.play();
        }
    }, 500);
}

function jsLoadComicStrip(comicStripName) {
    $('#comic').attr('src', 'Content/Images/' + comicStripName);
    $('#modal').fadeIn(500);
}

function jsLoadUrl(url) {
    window.open(url, "_blank");
}

/**
 * @description 判断浏览器是否竖屏
 */
function jsIsBroswerSP() {
    return window.orientation === 180 || window.orientation === 0;
}

/**
 * @description 判断是否苹果浏览器
 */
function jsIOSBroswer() {
    var UA = navigator.userAgent;
    if (UA.match(/iPad/) || UA.match(/iPhone/) || UA.match(/iPod/)) {
        return true;
    } else {
        return false;
    }
}

/**
 * @description 获取平台的类型
 * @return 0:Unity编辑器,1:非IOS平台，2:IOS平台
*/
function jsGetPlatformType() {

    return isIOSBrowser() ? 2 : 1;
}

function jsIsWXBrowser()
{
    var ua = navigator.userAgent.toLowerCase();
    var isWeixin = ua.indexOf('micromessenger') != -1;
    if (isWeixin) {
        return true;
    } else {
        return false;
    }
}

/**
 * @description 设置全屏显示
*/
function jsSetFullScreen() {
    unityInstance.SetFullscreen(1);
    //canvas.requestFullscreen();
}

/**
 * @description 判断是否为移动端浏览器
*/
function jsIsMobileBroswerSP() {
    if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        return true;
    }
    else {
        return false;
    }
}

/**
 * @description 关闭按钮单击事件
*/
function onBtnCloseClick() {
    //jsHideView();
    var divVideoPlayer = document.querySelector("#divVideoPlayer");
    var btnClose = document.querySelector("#btnClose");
    //var vdMain = document.querySelector("#vdPlayer");
    //vdMain.pause();
    divVideoPlayer.style.display = "none";
    btnClose.style.display = "none";
    gameInstance.SendMessage("H5Receiver", "HideView", "");
}

// 是否为最大化
var isModalMax = false;
/**
 * @description 最大化按钮单击事件
*/
function onBtnMaxClick() {
    isModalMax = !isModalMax;
    if (isModalMax) {
        $('#modal').removeClass('center-modal');
        $('#modal').addClass('maxmize-modal');
        $('#modal').fadeIn(500);
    }
    else {
        $('#modal').removeClass('maxmize-modal');
        $('#modal').addClass('center-modal');
        $('#modal').fadeIn(500);
    }
}

/**
 * @description 音频播放按钮单击事件
*/
function onBtnPlayClick() {
    console.log("play");
    var auMain = document.querySelector("#auMain");
    var btnPlay = document.querySelector("#btnPlay");

    if (auMain.paused) {
        btnPlay.style.backgroundImage = "url('../images/ui/pause.png')";
        auMain.load();
        auMain.play();
    }
    else {
        btnPlay.style.backgroundImage = "url('../images/ui/play.png')";
        auMain.pause();
    }
}

/**
 * @description 关闭按钮单击事件
*/
function onBtnSkipClick() {
    
    // jsLoadVideo("ZT0_3_1（台湾名称的由来）.mp4");

    //vdMain.pause();
    //vdMain.removeAttribute('src')
    //vdMain.remove();

    document.getElementById('boxProgress').style.display = "block";
    //btnSkip.style.display = "none";

    warningBanner.remove();

    if (newProgress > 100) {
        hideLoadingBar();
        gameInstance.SendMessage("H5Receiver", "EnterGame", "");
    }
    else {
        logo.style.backgroundImage = "url('images/loadingTW.jpg')";
        showImages();
    }
}
