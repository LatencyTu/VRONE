mergeInto(LibraryManager.library, {

  DoJs:function(core){
    console.log(UTF8ToString(core));
    eval(UTF8ToString(core));
  },
  Hello: function () {
    window.alert("Hello, world!");
  },
  IsMobileBroswer:function(){
    return jsIsMobileBroswerSP();
  },
  IsMobileBroswer2:function(){
    if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        return true;
    }
    else {
        return false;
    }
  },
  UnitySetFullscreen: function () {
    gameInstance.SetFullscreen(1);
  },
  UnityAlert: function (msg) {
    window.alert(UTF8ToString(msg));
  },
  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },

  PrintFloatArray: function (array, size) {
    for(var i = 0; i < size; i++)
    console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },

});