!function(n) {
    "use strict";
    function t() {
        eval("return 1");
    }
    function o(n) {
        return n + t();
    }
    console.log(o("a"));
}();