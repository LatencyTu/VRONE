stretchScreen();

var mask = document.querySelector("#mask");

// ����������ʾ�ĺ���
function handlePortraitOfMobile() {
    if (window.orientation === 180 || window.orientation === 0) {
        mask.style.display = "block";
    } else {
        mask.style.display = "none";

        setTimeout("stretchScreen()", 200); // ��ʱ���ܻ�ȡ�����º�����
    }
}

// ��ʼ������״̬����
handlePortraitOfMobile();

// �����������л��¼�
window.addEventListener("orientationchange", handlePortraitOfMobile);
window.addEventListener("resize", handlePortraitOfMobile);

//var evt = "onorientationchange" in window ? "orientationchange" : "resize";
//window.addEventListener(evt, function () {
//    // �����¼��е�����״̬����
//    handlePortraitOfMobile();
//}, false);




