var index = 1;     
var newProgress;    
var opa = 1;       
var loadImg = true;


function setOldProgresss(percent) {
    newProgress = Math.floor(percent * 100);
    setProgress(newProgress);
}

function setProgress(percent) {
    var box = document.getElementById('boxProgress');
    var bar = document.getElementById('bar');
    var text = document.getElementById('text');

    var allWidth = window.innerWidth; // parseInt(getStyle(box, 'width'));

    bar.innerHTML = percent + '%';
    text.innerHTML = percent + '%';

    bar.style.clip = 'rect(0px, ' + percent / 100 * allWidth + 'px, 40px, 0px)';
};

function getStyle(obj, attr) {
    if (obj.currentStyle) {
        return obj.currentStyle[attr];
    } else {
        return getComputedStyle(obj, false)[attr];
    }
}

var cusCount = 0;  
var endOfVideo = false;
function progressCustom() {
    // newProgress += Math.floor(Math.random() * 10);
    cusCount++;
    if (cusCount == 5) {
        newProgress = 101;
    };

    if (newProgress > 100) {
        setProgress(100);

        loadImg = false;
    }
    else {
        setProgress(newProgress);
        setTimeout("progressCustom()", 420);
    }
}

function hideLoadingBar() {
    opa -= 0.1;
    loadingBar.style.opacity = opa;
    if (opa <= 0) {
        loadingBar.style.display = "none";
    }
    else {
        setTimeout("hideLoadingBar()", 100);
    }
}

function showImages() {

    if (newProgress > 100) {
        hideLoadingBar();
        gameInstance.SendMessage("H5Receiver", "EnterGame", "");
    }
    else {       
        if (loadImg) {
            setTimeout("showImages()", 2000);
        }
    }
}

function stretchScreen() {
    var width = window.innerWidth; 
    var height = window.innerHeight; 
    // console.log(width + "," + height);

    canvas.style.width = width + "px";
    canvas.style.height = height + "px";

    loadingBar.style.width = width + "px";
    loadingBar.style.height = height + "px";
}
