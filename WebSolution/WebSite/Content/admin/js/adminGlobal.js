(function (adminGlobal, $, undefined) {

    adminGlobal.initPage = function () {
        initPartial();

        $(".btn-action").live("click", function () {
            var action = $(this).data("action");
            if (action) {
                location.href = action;
            }
        });

        $(".btn-action-partial").live("click", function () {
            btnActionPartialClick($(this));
        })
        $(".tab-controller").live("click", function () {
            var sender = $(this);
            sender.parent().find(".tab-controller").each(function () {
                var s = $(this);
                s.removeClass("active-sub-header");
                var target = $("." + s.data("target"));
                if (target.find(".input-validation-error").size() > 0) {
                    s.addClass("field-validation-error");
                } else {
                    s.removeClass("field-validation-error");
                }
                target.hide();
            });
            sender.addClass("active-sub-header");
            $("." + sender.data("target")).show();
            initCLEditor();
        });
        $(".btn-color-box").live("click", function () {
            btnColorBoxClick($(this));
        });
        $("#message-wrapper .close").live("click", function () {
            $("#message-wrapper").hide();
        });
        $(".show-editor").live("click", function () {
            var target = $("#" + $(this).data("target"));
            target.siblings().hide();
            target.show();
        });
        $(".crop-enabled, .crop-enabled-btn").live("click", function () {
            showCrop($(this));
        });
        adminGlobal.initAfterPartial();
    }

    adminGlobal.initAfterPartial = function () {
        $(".date-picker").datepicker(
        {
            showOn: "both",
            buttonImage: $("#date-picker-image").val(),
            buttonImageOnly: true,
            dateFormat: "yy-mm-dd",
            dayNamesMin: ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"],
            firstDay: 1,
            buttonText: ''
        });
        initFileUploader();
        $(".tab-controller").first().click();
        initCLEditor();
    }

    adminGlobal.clearAllTextInput = function (container) {
        if (container.find(".input-validation-error").size() == 0) {
            container.find(":text, textarea").val("");
        }
    }

    adminGlobal.showSuccessMessage = function () {
        var messageWrapper = $("#message-wrapper");
        if (messageWrapper.size() > 0) {
            //messageWrapper.removeClass("fail-message-text").addClass("success-message-text").text("Completed");
            messageWrapper.show();
            setTimeout(function () {
                messageWrapper.find(".close").click();
            }, 5000);
        } else {
            alertPopup("<table width='100%'><tr><td class='success-message-image'></td><td class='success-message-text'>Completed.</td></tr></table>");
        }
    }

    adminGlobal.showFailMessage = function () {
        alertPopup("<table width='100%'><tr><td class='fail-message-image'></td><td class='fail-message-text'>Sorry, an error occurs, please try again</td></tr></table>");
    }

    function alertPopup(message) {
        $("#btn-color-box-trigger").colorbox({
            html: message,
            width: 310,
            height: 155
        });
        $("#btn-color-box-trigger").click();
    }

    function btnColorBoxClick(sender) {
        var action = sender.data("action");
        $.ajax({
            type: "POST",
            url: action,
            data: {
                _: new Date().getTime()
            },
            success: function (result) {
                $("#btn-color-box-trigger").colorbox({
                    html: result,
                    width: 550,
                    height: 375
                });
                $("#btn-color-box-trigger").click();
                adminGlobal.initAfterPartial();
            },
            error: function () {
                adminGlobal.showFailMessage();
            }
        });
    }

    function initCLEditor() {
        var items = $("textarea.rich-text:visible");
        if (items.size() > 0) {
            items.cleditor({
                //                controls:     // controls to add to the toolbar
                //                    "bold italic underline strikethrough subscript superscript | " +
                //                    "color highlight removeformat | " +
                //                    "undo redo | " +
                //                    "link unlink | cut copy paste pastetext | source"
                controls:     // controls to add to the toolbar
                    "bold italic underline | pastetext source"
            });
            $(".cleditorMain").each(function () {
                var sender = $(this);
                if (sender.find(".input-validation-error").size() > 0) {
                    sender.addClass("input-validation-error");
                }
            });
        }
    }

    function initFileUploader() {
        var saveFileAction = $("#save-file-action");
        var fileLoaders = $(".file-upload-wrapper");
        fileLoaders.hide();
        if (saveFileAction.size() > 0) {
            fileLoaders.each(function () {
                var sender = $(this);
                sender.find(".files").empty();
                sender.find(":file").val("");
            });
            var uploader = $("#file-uploader");
            uploader.find(".save-files").data("action", saveFileAction.val()).data("crop-options", saveFileAction.data("crop-options"));
            var backToIndex = uploader.find(".back-to-index");
            var backToIndexAction = $("#back-to-index-action");
            if (backToIndexAction.size() > 0) {
                backToIndex.data("action", backToIndexAction.val());
                backToIndex.show();
            } else {
                backToIndex.hide();
            }

            var form = uploader.find("form");
            form.fileupload();
            var maxFileSize = 0;
            var acceptFileTypes;
            switch (saveFileAction.data("file-type")) {
                case "music":
                    maxFileSize = 200000000;
                    acceptFileTypes = /(\.|\/)(mp3)$/i;
                    break;
                case "video":
                    maxFileSize = 500000000;
                    acceptFileTypes = /(\.|\/)(mp4)$/i;
                    break;
                default:
                    maxFileSize = 5000000;
                    acceptFileTypes = /(\.|\/)(jpe?g|png)$/i;
            }
            form.fileupload('option', {
                maxFileSize: maxFileSize,
                resizeMaxWidth: 1920,
                resizeMaxHeight: 1200,
                acceptFileTypes: acceptFileTypes
            });
            uploader.show();
        }
        if ($.browser.msie) {
            var btnUploadFile = $(".fileinput-button :file");
            btnUploadFile.css({ 'border-width': '0', 'font-size': '31px' });
        }
    }

    function btnActionPartialClick(sender) {
        var showSuccessMessage = sender.hasClass("show-success-message");
        var needClose = sender.hasClass("need-close");
        var action = sender.data("action");
        var goAhead = true;
        if (sender.hasClass("need-confirm")) {
            var tip = sender.data("tip");
            var warn = "";
            if (tip) {
                warn = "Are you sure to delete [" + tip + "]?";
            } else {
                warn = "Are you sure to delete this item?";
            }
            goAhead = confirm(warn);
        }
        var data = {};
        if (sender.hasClass("save-files")) {
            var uploader = sender.parent().parent();
            var imgTags = uploader.find(".preview img");
            if (imgTags.size() == 0) {
                var messageWrapper = $(".save-file-message-wrapper");
                messageWrapper.text("No files to save");
                setTimeout(function () {
                    messageWrapper.text("");
                }, 5000);
                return;
            }
            var images = "";
            imgTags.each(function () {
                var imageUrl = $(this).attr("src");
                var questionMarkIndex = imageUrl.indexOf("?");
                if (questionMarkIndex > -1) {
                    imageUrl = imageUrl.substring(0, questionMarkIndex);
                }
                images += (imageUrl + "|");
            });
            var confirmJson = confirmImageSize(images, sender.data("crop-options"));
            if (!confirmJson.ConfirmResult) {
                return;
            } else {
                images = confirmJson.FilteredImages;
            }
            data = {
                images: images,
                _: new Date().getTime()
            }
        } else if (sender.data("form")) {
            data = $("#" + sender.data("form")).serializeArray();
        }
        else {
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
                    if (needClose) {
                        $("#cboxClose").click();
                        setTimeout(function () {
                            $("#" + targetId).html(result);
                            adminGlobal.initAfterPartial();
                            if (showSuccessMessage && $(".input-validation-error").size() == 0) {
                                adminGlobal.showSuccessMessage();
                            }
                        }, 500);
                    } else {
                        $("#" + targetId).html(result);
                        adminGlobal.initAfterPartial();
                        if (showSuccessMessage && $(".input-validation-error").size() == 0) {
                            adminGlobal.showSuccessMessage();
                        }
                    }
                },
                error: function () {
                    adminGlobal.showFailMessage();
                }
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
                        adminGlobal.showFailMessage();
                    },
                    async: false
                });
            });
        }
    }

    function showCrop(sender) {
        var imgWrapper;
        if (sender.hasClass("crop-enabled-btn")) {
            imgWrapper = sender.parent().siblings(".crop-enabled").first();
        } else {
            imgWrapper = sender;
        }
        var cropOptions = imgWrapper.data("crop-options");

        var imgTag = imgWrapper.find("img");
        var rawImgSrc = imgTag.attr("src");
        var imgSrc = rawImgSrc;
        var spliterIndex = imgSrc.indexOf("?");
        if (spliterIndex > -1) {
            imgSrc = imgSrc.substr(0, spliterIndex);
        }
        $.ajax({
            type: "POST",
            url: $("#image-crop-canvas-url").val(),
            data: {
                imageSrc: imgSrc,
                _: new Date().getTime()
            },
            success: function (res) {
                $("#btn-color-box-trigger").colorbox({
                    html: res,
                    transition: "none",
                    scrolling: false,
                    scalePhotos: false,
                    onComplete: function () {
                        $("#crop-image").Jcrop({
                            onChange: fillCoords,
                            onSelect: fillCoords,
                            onRelease: clearCoords,
                            setSelect: [0, 0, 10000, 10000],
                            aspectRatio: cropOptions.aspectRatio,
                            minSize: [cropOptions.minWidth, cropOptions.minHeight],
                            maxSize: [cropOptions.maxWidth, cropOptions.maxHeight]
                        });
                        $("#btn-crop-image").click(function () {
                            $.ajax({
                                type: "POST",
                                url: $(this).data("action"),
                                data: $("#corp-form").serializeArray(),
                                async: false,
                                success: function () {
                                    $("#cboxClose").click();
                                    imgWrapper.html("");
                                    imgWrapper.html("<img src='" + rawImgSrc + "&_=" + new Date().getTime() + "' />")
                                }
                            })
                        });
                    }
                });
                $("#btn-color-box-trigger").click();
            }
        });
    }

    function fillCoords(c) {
        $('#crop-x').val(c.x);
        $('#crop-y').val(c.y);
        $('#crop-width').val(c.w);
        $('#crop-height').val(c.h);
    };

    function clearCoords() {
        $('#corp-form input').val('');
    };

    function confirmImageSize(images, cropOptions) {
        var result = {
            ConfirmResult: true,
            FilteredImages: images
        };
        if (cropOptions && (cropOptions.minWidth > 0 || cropOptions.minHeight > 0)) {
            $.ajax({
                type: "POST",
                url: $("#confirm-image-size-url").val(),
                data: {
                    images: images,
                    minWidth: cropOptions.minWidth,
                    minHeight: cropOptions.minHeight,
                    _: new Date().getTime()
                },
                async: false,
                dataType: "json",
                success: function (response) {
                    if (!response.IsValid) {
                        var confirmString = "[" + response.FileNames + "] is too small - minimum size: " + cropOptions.minWidth + " x " + cropOptions.minHeight + ". Continue?";
                        result.ConfirmResult = confirm(confirmString);
                    }
                    result.FilteredImages = response.FilteredImages;
                }
            });
        }
        return result;
    }

} (window.adminGlobal = window.adminGlobal || {}, jQuery));