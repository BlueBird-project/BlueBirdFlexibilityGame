const MobileDetection = {
    IsMobile: function() {
        return Module.SystemInfo.mobile;
    },

    IsLandscape: function() {
        return window.innerWidth > window.innerHeight;
    },

    RequestFullscreen: function() {
        var element = document.documentElement;

        if (element.requestFullscreen) {
            element.requestFullscreen();
        }
        else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen();
        }
    }
};

mergeInto(LibraryManager.library, MobileDetection);