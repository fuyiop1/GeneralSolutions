(function (global, $, undefined) {

    global.initPage = function () {
        initPartial();
    }


    function btnActionPartialClick(sender) {
        var action = sender.data("action");
        var goAhead = true;
        var data = {};
        if (sender.data("form")) {
            data = $("#" + sender.data("form")).serializeArray();
        } else {
            data = {
                _: new Date().getTime()
            };
        }
        if (goAhead) {
            $.ajax({
                type: "POST",
                url: action,
                data: data,
                success: function (result) {
                    var targetId = "partial-content-wrapper";
                    if (sender.data("target")) {
                        targetId = sender.data("target");
                    }
                    $("#" + targetId).html(result);
                    global.initAfterPartial();
                },
                error: function () {
                },
                async: false
            });
        }
    }

    function initPartial() {
        var trigger = $(".init-query-trigger");
        if (trigger.size() > 0) {
            trigger.each(function () {
                var sender = $(this);
                $.ajax({
                    url: sender.data("action"),
                    type: "POST",
                    data: sender.parent("form").serializeArray(),
                    success: function (result) {
                        var targetId = "partial-content-wrapper";
                        if (sender.data("target")) {
                            targetId = sender.data("target");
                        }
                        $("#" + targetId).html(result);
                    },
                    error: function () {
                    },
                    async: false
                });
            });
        }
    }

    global.initAfterPartial = function () {
        $(".btn-action-partial").click(function () {
            btnActionPartialClick($(this));
        });
        $(".auto-height").each(function () {
            autoSizeTextarea(this);
        });
        initSliders();
    }

    global.initScrollPage = function () {
        initAutoFloatPage();
        loadItems();
        setTimeout(function () {
            bindWindowScroll();
        }, 1000)
    }

    function initAutoFloatPage() {
        var $container = $('#item-list-wrapper');
        if ($container.hasClass("need-re-layout")) {
            $container.imagesLoaded(function () {
                $container.masonry({
                    itemSelector: '.board',
                    columnWidth: 282,
                    isFitWidth: true,
                    isAnimated: false
                });
            });
        }
    }

    function windowScroll() {
        if ($("#is-end").val() == "true") {
            return;
        }
        if ($(document).scrollTop() >= $(document).height() - $(window).height()) {
            loadItems();
        }
    }

    function bindWindowScroll() {
        $(window).off("scroll");
        $(window).scroll(function () {
            windowScroll();
        });
    }

    var isItemLoading = false;

    function loadItems() {
        if (isItemLoading || $("#load-item-form").size() == 0) {
            return;
        }
        isItemLoading = true;
        $.ajax({
            type: "POST",
            url: $("#index-partial-action").val(),
            data: $("#load-item-form").serializeArray(),
            success: function (result) {
                var res = $(result);
                res.find(".hidden-update-wrapper").children().each(function () {
                    var item = $(this);
                    $("#" + item.data("target")).val(item.val());
                });
                if ($("#is-end").val() == "true") {
                    $("#cut-off-publish-time").val("");
                    $("#loading-more-wrapper").hide();
                    $("#end-wrapper").show();
                    isItemLoading = false;
                } else {
                    $("#loading-more-wrapper").show();
                    $("#end-wrapper").hide();
                }
                var $newElems = $(res.find(".partial-item-wrapper").children());
                $newElems.hide();
                var $container = $("#item-list-wrapper");
                $newElems.appendTo($container);
                if ($container.hasClass("need-re-layout")) {
                    $newElems.imagesLoaded(function () {
                        var container = $("#item-list-wrapper")
                        container.masonry('appended', $newElems, true);
                        setTimeout(function () {
                            container.masonry();
                            $newElems.show();
                            loadItemsAgain();
                        }, 100);
                    });
                } else {
                    $newElems.show();
                    loadItemsAgain();
                }
            }
        });
    }

    function loadItemsAgain() {
        isItemLoading = false;
        if ($(window).height() >= $(document.body).height() && $("#is-end").val() != "true") {
            loadItems();
        }
    }

    function autoSizeTextarea(item) {
        var height = 0;
        var style = item.style;
        var minHeight = 0;
        var maxHeight = 1000000;
        if (item.scrollHeight > minHeight) {
            if (maxHeight && item.scrollHeight > maxHeight) {
                height = maxHeight;
                style.overflowY = 'scroll';
            } else {
                height = item.scrollHeight;
                style.overflowY = 'hidden';
            }
            style.height = height + 'px';
        }
        $(item).attr("readonly", "readonly");
    }

    function initSliders() {
        var commonSlidersContainer = $("#con");
        if ($("#album-sepecific-image-wrapper").size() > 0) {
            commonSlidersContainer.hide();
        } else {
            commonSlidersContainer.show();
        }
    }

} (window.global = window.global || {}, jQuery));