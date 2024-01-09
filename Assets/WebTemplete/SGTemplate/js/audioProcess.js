var adMain = document.querySelector("#adMain");
var adAnother = document.querySelector("#adAnother");

// 播放第index展厅背景音乐
function jsPlayBg(index) {

    adMain.src = "StreamingAssets/Audios/ZT" + index + ".mp3";
    // adMain.play();

    adMain.load();
    adMain.currentTime = 0;
    let playPromise = adMain.play();
    if (playPromise) {
        playPromise.then(() => {
            audio.play();
        }).catch(() => {
            
        })
    }
}

/// <summary>
/// 暂停背景音乐
/// </summary>
function jsPauseBackgroundMusic() {
    adMain.pause();
}

/// <summary>
/// 恢复背景音乐
/// </summary>
function jsResumeBackgroundMusic() {
    adMain.play();
}

/// <summary>
/// 设置背景音乐静音
/// </summary>
function jsSetBackgroundMute() {
    adMain.muted = !adMain.muted;
}

/// <summary>
/// 播放音效(默认在摄像机位置播放)
/// </summary>
function jsPlayAudio(audioName) {
    adAnother.volume = 0.5;
    adAnother.src = "StreamingAssets/Audios/" + audioName + ".mp3";
    adAnother.play();
}

/// <summary>
/// 停止音效
/// </summary>
function jsStopAudio() {
    adAnother.stop();
}
