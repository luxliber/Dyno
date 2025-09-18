(function ($) {

    'use strict';

    jQuery.each(hugeit_gen_resp_lightbox_obj, function (index, value) {
        if (value.indexOf('true') > -1 || value.indexOf('false') > -1)
            hugeit_gen_resp_lightbox_obj[index] = value == "true";
    });

    function Lightbox(element, options) {

        this.el = element;
        this.$element = $(element);
        this.$body = $('body');
        this.objects = {};
        this.lightboxModul = {};
        this.$item = '';
        this.$cont = '';
        this.$items = this.$body.find('a.responsive_lightbox');

        this.settings = $.extend({}, this.constructor.defaults, options);

        this.init();

        return this;
    }

    Lightbox.defaults = {
        idPrefix: 'rwd-',
        classPrefix: 'rwd-',
        attrPrefix: 'data-',
        slideAnimationType: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_slideAnimationType, /*  effect_1   effect_2    effect_3
         effect_4   effect_5    effect_6
         effect_7   effect_8    effect_9   */
        lightboxView: hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView,              //  view1, view2, view3, view4, view5, view6, view7
        speed: hugeit_resp_lightbox_obj.hugeit_lightbox_speed_new,
        width: hugeit_resp_lightbox_obj.hugeit_lightbox_width_new + '%',
        height: hugeit_resp_lightbox_obj.hugeit_lightbox_height_new + '%',
        videoMaxWidth: '700',
        sizeFix: true, //not for option
        overlayDuration: +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_overlayDuration,
        slideAnimation: true, //not for option
        overlayClose: hugeit_resp_lightbox_obj.hugeit_lightbox_overlayClose_new,
        loop: hugeit_resp_lightbox_obj.hugeit_lightbox_loop_new,
        escKey: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_escKey_new,
        keyPress: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_keyPress_new,
        arrows: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_arrows,
        mouseWheel: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_mouseWheel,
        download: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_download,
        showTitle: hugeit_resp_lightbox_obj.hugeit_lightbox_showTitle === 'true',
        showDesc: hugeit_resp_lightbox_obj.hugeit_lightbox_showDesc === 'true',
        showBorder: hugeit_resp_lightbox_obj.hugeit_lightbox_showBorder === 'true',
        showCounter: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_showCounter,
        defaultTitle: '',  //some text
        preload: 10,  //not for option
        showAfterLoad: true,  //not for option
        nextHtml: '',  //not for option
        prevHtml: '',  //not for option
        sequence_info: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_sequence_info,
        sequenceInfo: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_sequenceInfo,
        slideshow: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_slideshow_new,
        slideshowAuto: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_slideshow_auto_new,
        slideshowSpeed: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_slideshow_speed_new,
        slideshowStart: '',  //not for option
        slideshowStop: '',   //not for option
        hideControlOnEnd: false,  //not for option
        watermark: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark,
        socialSharing: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_socialSharing,
        titlePos: hugeit_resp_lightbox_obj.hugeit_lightbox_title_pos,
        fullwidth: hugeit_resp_lightbox_obj.hugeit_lightbox_fullwidth_effect === 'true',
        zoomLogo: hugeit_resp_lightbox_obj.hugeit_lightbox_zoomlogo,
        wURL: hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_link,
        watermarkURL: hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_url,
        wURLnewTab: hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_url_new_tab,
        openCloseType: hugeit_resp_lightbox_obj.lightbox_open_close_effect,
        thumbnail: hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs === 'true',
        share: {
            facebookButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_facebookButton,
            twitterButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_twitterButton,
            googleplusButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_googleplusButton,
            pinterestButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_pinterestButton,
            linkedinButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_linkedinButton,
            tumblrButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_tumblrButton,
            redditButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_redditButton,
            bufferButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_bufferButton,
            diggButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_diggButton,
            vkButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_vkButton,
            yummlyButton: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_yummlyButton
        }
    };

    Lightbox.prototype.init = function () {

        var $object = this,
            $hash,
            $openCloseType;

        switch(this.settings.openCloseType){
            case '0':
                $openCloseType = {
                    0: 'open_0',
                    1: 'close_0'
                };
                break;
            case '1':
                $openCloseType = {
                    0: 'open_1',
                    1: 'close_1'
                };
                break;
            case '2':
                $openCloseType = {
                    0: 'open_1_r',
                    1: 'close_1_r'
                };
                break;
            case '3':
                $openCloseType = {
                    0: 'open_2',
                    1: 'close_2'
                };
                break;
            case '4':
                $openCloseType = {
                    0: 'open_2_r',
                    1: 'close_2_r'
                };
                break;
            case '5':
                $openCloseType = {
                    0: 'open_3',
                    1: 'close_3'
                };
                break;
            case '6':
                $openCloseType = {
                    0: 'open_3_r',
                    1: 'close_3_r'
                };
                break;
            case '7':
                $openCloseType = {
                    0: 'open_4',
                    1: 'close_4'
                };
                break;
            case '8':
                $openCloseType = {
                    0: 'open_4_r',
                    1: 'close_4_r'
                };
                break;
            case '9':
                $openCloseType = {
                    0: 'open_5',
                    1: 'close_5'
                };
                break;
            case '10':
                $openCloseType = {
                    0: 'open_5_r',
                    1: 'close_5_r'
                };
                break;
        }

        if(this.settings.lightboxView !== 'view7'){
            this.settings.openCloseType = $openCloseType;
        } else {
            this.settings.openCloseType = {0: 'open_1', 1: 'close_1'};
            this.settings.slideAnimationType = 'effect_2';
        }

        $hash = window.location.hash;

        ($object.settings.watermark && $('.watermark').watermark());

        if ($hash.indexOf('lightbox&') > 0) {
            $object.index = parseInt($hash.split('&slide=')[1], 10) - 1;

            $object.$body.addClass('rwd-share');
            if (!$object.$body.hasClass('rwd-on')) {
                setTimeout(function () {
                    $object.build($object.index);
                }, 900);
                $object.$body.addClass('rwd-on');
            }
        }

        (($object.settings.preload > $object.$items.length) && ($object.settings.preload = $object.$items.length));

        $object.$items.on('click.rwdcustom', function (event) {

            event = event || window.event;
            event.preventDefault ? event.preventDefault() : (event.returnValue = false);

            $object.index = $object.$items.index(this);

            if (!$object.$body.hasClass($object.settings.classPrefix + 'on')) {
                $object.build($object.index);
                $object.$body.addClass($object.settings.classPrefix + 'on');
            }

        });

        $object.$body.on('click', function () {
            $object.$_y_ = window.pageYOffset;
        });

        switch (this.settings.zoomLogo) {
            case '1':
                $object.$body.addClass('rwd-zoomGlass');
                break;
            case '2':
                $object.$body.addClass('rwd-zoomHand');
                break;
        }

    };

    Lightbox.prototype.build = function (index) {

        var $object = this;

        $object.structure(index);

        $object.lightboxModul['modul'] = new $.fn.lightbox.lightboxModul['modul']($object.el);

        $object.slide(index, false, false);

        ($object.settings.keyPress && $object.addKeyEvents());

        if ($object.$items.length > 1) {

            $object.arrow();

            ($object.settings.mouseWheel && $object.mousewheel());

            ($object.settings.slideshow && $object.slideShow());

        }

        $object.counter();

        $object.closeGallery();

        $object.$cont.on('click.rwd-container', function () {

            $object.$cont.removeClass($object.settings.classPrefix + 'hide-items');

        });

        $('.shareLook').on('click.rwd-container', function () {
            $(this).css({'display': 'none'});
            $('.rwd-share-buttons').css({'display': 'block'});
            setTimeout(function(){
                $('.shareLook').css({'display' : 'block'});
                $('.rwd-share-buttons').css({'display' : 'none'});
            }, 9000);
        });

        if(!this.settings.thumbnail){
            $object.calculateDimensions(index);
        } else {
            setTimeout(function(){
                $object.calculateDimensions(index);
            }, 800);
        }

        if(this.settings.lightboxView === 'view7'){
            setTimeout(function(){
                $('.view7_share, .rwd-close-bar, .rwd-toolbar').css({
                    visibility: 'visible',
                    opacity: '1'
                });

                $object.calculateDimensions(index);
            }, 1100);
            
            jQuery('.rwd-close').hover(function(){
                $('.rwd-cont').addClass('close_hover_on');
            }, function(){
                $('.rwd-cont').removeClass('close_hover_on');
            });

            var $w, $l;

            if($('.rwd-container').width() > 768){
                $('.rwd-view7-title').width($('.rwd-container').width() * 0.2 - 46);
                $('.rwd-view7-desc').width($('.rwd-container').width() * 0.2 - 36);

                setTimeout(function(){
                    $('.rwd-view7-desc').height($('.view7_inner').height() - 79);
                }, 200);

                $w = 256;
                $l = $('.tool_bar .rwd-icon:not(.zoom_cont)').length;

                if(hugeit_resp_lightbox_obj.hugeit_lightbox_zoom && hugeit_resp_lightbox_obj.hugeit_lightbox_zoomtype === '1'){
                    $('.zoom_cont').css({
                        'max-width': '220px'
                    });

                    $w = 220;
                } else {
                    $('.zoom_cont').css({
                        'max-width': '145px'
                    });

                    $w = 145;
                }

                $('.tool_bar').width(50 * $l + $w);
            } else {
                $('.tool_bar').css('width', '80%');
            }
        }
    };

    Lightbox.prototype.structure = function (index) {

        var $object = this, list = '', controls = '', i,
            subHtmlCont1 = '', subHtmlCont2 = '', subHtmlCont3 = '',
            close1 = '', close2 = '', close3 = '', socialIcons = '',
            template, $arrows, $next, $prev,
            $_next, $_prev, $close_bg, $download_bg, $download_bg_, $contInner, $view;

        $view = (this.settings.lightboxView === 'view6') ? 'rwd-view6' : '';

        this.$body.append(
            this.objects.overlay = $('<div class="' + this.settings.classPrefix + 'overlay ' + $view + '"></div>')
        );
        this.objects.overlay.css('transition-duration', this.settings.overlayDuration + 'ms');

        var $wURL = '',
            $target = '';

        if($object.settings.watermark && $object.settings.wURL && hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_text){
            if($object.settings.wURLnewTab){
                $target = 'target="_blank"';
            }
            $wURL = '<a href="' + $object.settings.watermarkURL + '" class="w_url" ' + $target + '></a>';
        }

        for (i = 0; i < this.$items.length; i++) {
            list += '<div class="' + this.settings.classPrefix + 'item">' + $wURL + '</div>';
        }

        $close_bg = '<svg class="close_bg" width="16px" height="16px" fill="#999" viewBox="-341 343.4 15.6 15.6">' +
            '<path d="M-332.1,351.2l6.5-6.5c0.3-0.3,0.3-0.8,0-1.1s-0.8-0.3-1.1,0l-6.5,6.5l-6.5-6.5c-0.3-0.3-0.8-0.3-1.1,0s-0.3,0.8,0,1.1l6.5,6.5l-6.5,6.5c-0.3,0.3-0.3,0.8,0,1.1c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2l6.5-6.5l6.5,6.5c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2c0.3-0.3,0.3-0.8,0-1.1L-332.1,351.2z"/>' +
            '</svg>';

        switch (this.settings.lightboxView) {
            case 'view1':
            default:
                $_next = '<svg class="next_bg" width="22px" height="22px" fill="#999" viewBox="-333 335.5 31.5 31.5" >' +
                    '<path d="M-311.8,340.5c-0.4-0.4-1.1-0.4-1.6,0c-0.4,0.4-0.4,1.1,0,1.6l8,8h-26.6c-0.6,0-1.1,0.5-1.1,1.1s0.5,1.1,1.1,1.1h26.6l-8,8c-0.4,0.4-0.4,1.2,0,1.6c0.4,0.4,1.2,0.4,1.6,0l10-10c0.4-0.4,0.4-1.1,0-1.6L-311.8,340.5z"/>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="22px" fill="#999" viewBox="-333 335.5 31.5 31.5" >' +
                    '<path d="M-322.7,340.5c0.4-0.4,1.1-0.4,1.6,0c0.4,0.4,0.4,1.1,0,1.6l-8,8h26.6c0.6,0,1.1,0.5,1.1,1.1c0,0.6-0.5,1.1-1.1,1.1h-26.6l8,8c0.4,0.4,0.4,1.2,0,1.6c-0.4,0.4-1.1,0.4-1.6,0l-10-10c-0.4-0.4-0.4-1.1,0-1.6L-322.7,340.5z"/>' +
                    '</svg>';
                if(this.settings.showTitle){
                    subHtmlCont1 = '<div class="' + this.settings.classPrefix + 'title"></div>';
                }
                close1 = '<span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>';
                break;
            case 'view2':
                $_next = '<svg class="next_bg" width="22px" height="22px" fill="#999" viewBox="-123 125.2 451.8 451.8" >' +
                    '<g><path d="M222.4,373.4L28.2,567.7c-12.4,12.4-32.4,12.4-44.8,0c-12.4-12.4-12.4-32.4,0-44.7l171.9-171.9L-16.6,179.2c-12.4-12.4-12.4-32.4,0-44.7c12.4-12.4,32.4-12.4,44.8,0l194.3,194.3c6.2,6.2,9.3,14.3,9.3,22.4C231.7,359.2,228.6,367.3,222.4,373.4z"/></g>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="22px" fill="#999" viewBox="-123 125.2 451.8 451.8" >' +
                    '<g><path d="M-25.9,351.1c0-8.1,3.1-16.2,9.3-22.4l194.3-194.3c12.4-12.4,32.4-12.4,44.8,0c12.4,12.4,12.4,32.4,0,44.7L50.5,351.1L222.4,523c12.4,12.4,12.4,32.4,0,44.7c-12.4,12.4-32.4,12.4-44.7,0L-16.6,373.4C-22.8,367.3-25.9,359.2-25.9,351.1z"/></g>' +
                    '</svg>';
                if(this.settings.showTitle){
                    subHtmlCont2 = '<div class="' + this.settings.classPrefix + 'title"></div>';
                }
                close2 = '<div class="barCont"></div><span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>';
                break;
            case 'view3':
                $_next = '<svg class="next_bg" width="22px" height="22px" fill="#999" viewBox="-104 105.6 490.4 490.4" >' +
                    '<g><g><path d="M141.2,596c135.2,0,245.2-110,245.2-245.2s-110-245.2-245.2-245.2S-104,215.6-104,350.8S6,596,141.2,596z M141.2,130.1c121.7,0,220.7,99,220.7,220.7s-99,220.7-220.7,220.7s-220.7-99-220.7-220.7S19.5,130.1,141.2,130.1z"/>' +
                    '<path d="M34.7,363.1h183.4l-48,48c-4.8,4.8-4.8,12.5,0,17.3c2.4,2.4,5.5,3.6,8.7,3.6s6.3-1.2,8.7-3.6l68.9-68.9c4.8-4.8,4.8-12.5,0-17.3l-68.9-68.9c-4.8-4.8-12.5-4.8-17.3,0s-4.8,12.5,0,17.3l48,48H34.7c-6.8,0-12.3,5.5-12.3,12.3C22.4,357.7,27.9,363.1,34.7,363.1z"/></g></g>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="22px" fill="#999" viewBox="-104 105.6 490.4 490.4" >' +
                    '<g><g><path d="M141.2,596c135.2,0,245.2-110,245.2-245.2s-110-245.2-245.2-245.2S-104,215.6-104,350.8S6,596,141.2,596z M141.2,130.1c121.7,0,220.7,99,220.7,220.7s-99,220.7-220.7,220.7s-220.7-99-220.7-220.7S19.5,130.1,141.2,130.1z"/>' +
                    '<path d="M94.9,428.4c2.4,2.4,5.5,3.6,8.7,3.6s6.3-1.2,8.7-3.6c4.8-4.8,4.8-12.5,0-17.3l-48-48h183.4c6.8,0,12.3-5.5,12.3-12.3c0-6.8-5.5-12.3-12.3-12.3H64.3l48-48c4.8-4.8,4.8-12.5,0-17.3c-4.8-4.8-12.5-4.8-17.3,0l-68.9,68.9c-4.8,4.8-4.8,12.5,0,17.3L94.9,428.4z"/></g></g>' +
                    '</svg>';
                if(this.settings.showTitle){
                    subHtmlCont1 = '<div class="' + this.settings.classPrefix + 'title"></div>';
                }
                close1 = '<span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>';
                break;
            case 'view4':
                $_next = '<svg class="next_bg" width="22px" height="22px" fill="#999" viewBox="-123 125.2 451.8 451.8" >' +
                    '<g><path d="M222.4,373.4L28.2,567.7c-12.4,12.4-32.4,12.4-44.8,0c-12.4-12.4-12.4-32.4,0-44.7l171.9-171.9L-16.6,179.2c-12.4-12.4-12.4-32.4,0-44.7c12.4-12.4,32.4-12.4,44.8,0l194.3,194.3c6.2,6.2,9.3,14.3,9.3,22.4C231.7,359.2,228.6,367.3,222.4,373.4z"/></g>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="22px" fill="#999" viewBox="-123 125.2 451.8 451.8" >' +
                    '<g><path d="M-25.9,351.1c0-8.1,3.1-16.2,9.3-22.4l194.3-194.3c12.4-12.4,32.4-12.4,44.8,0c12.4,12.4,12.4,32.4,0,44.7L50.5,351.1L222.4,523c12.4,12.4,12.4,32.4,0,44.7c-12.4,12.4-32.4,12.4-44.7,0L-16.6,373.4C-22.8,367.3-25.9,359.2-25.9,351.1z"/></g>' +
                    '</svg>';
                $close_bg = '<svg class="close_bg" width="16px" height="16px" fill="#999" viewBox="-341 343.4 15.6 15.6">' +
                    '<path d="M-332.1,351.2l6.5-6.5c0.3-0.3,0.3-0.8,0-1.1s-0.8-0.3-1.1,0l-6.5,6.5l-6.5-6.5c-0.3-0.3-0.8-0.3-1.1,0s-0.3,0.8,0,1.1l6.5,6.5l-6.5,6.5c-0.3,0.3-0.3,0.8,0,1.1c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2l6.5-6.5l6.5,6.5c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2c0.3-0.3,0.3-0.8,0-1.1L-332.1,351.2z"/>' +
                    '</svg>';
                if(this.settings.showTitle){
                    subHtmlCont2 = '<div class="' + this.settings.classPrefix + 'title"></div>';
                }
                close1 = '<span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>';
                break;
            case 'view5':
            case 'view6':
                $_next = '<svg class="next_bg" width="22px" height="44px" fill="#999" x="0px" y="0px"' +
                    'viewBox="0 0 40 70" style="enable-background:new 0 0 40 70;" xml:space="preserve">' +
                    '<path id="XMLID_2_" class="st0" d="M3.3,1.5L1.8,2.9l31.8,31.8c0.5,0.5,0.5,0.9,0,1.4L1.8,67.9l1.5,1.4c0.3,0.5,0.9,0.5,1.4,0' +
                    'l33.2-33.2c0.3-0.5,0.3-0.9,0-1.4L4.7,1.5C4.3,1,3.6,1,3.3,1.5L3.3,1.5z"/>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="44px" fill="#999" x="0px" y="0px"' +
                    'viewBox="0 0 40 70" style="enable-background:new 0 0 40 70;" xml:space="preserve">' +
                    '<path id="XMLID_2_" class="st0" d="M37.1,68.9l1.5-1.4L6.8,35.7c-0.3-0.5-0.3-0.9,0-1.4L38.6,2.5l-1.5-1.4c-0.3-0.5-0.9-0.5-1.2,0' +
                    'L2.5,34.3c-0.3,0.5-0.3,0.9,0,1.4l33.4,33.2C36.2,69.4,36.8,69.4,37.1,68.9L37.1,68.9z"/>' +
                    '</svg>';
                $close_bg = '<svg class="close_bg" width="16px" height="16px" fill="#999" viewBox="-341 343.4 15.6 15.6">' +
                    '<path d="M-332.1,351.2l6.5-6.5c0.3-0.3,0.3-0.8,0-1.1s-0.8-0.3-1.1,0l-6.5,6.5l-6.5-6.5c-0.3-0.3-0.8-0.3-1.1,0s-0.3,0.8,0,1.1l6.5,6.5l-6.5,6.5c-0.3,0.3-0.3,0.8,0,1.1c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2l6.5-6.5l6.5,6.5c0.1,0.1,0.3,0.2,0.5,0.2s0.4-0.1,0.5-0.2c0.3-0.3,0.3-0.8,0-1.1L-332.1,351.2z"/>' +
                    '</svg>';
                if(this.settings.showTitle){
                    subHtmlCont3 += '<div class="' + this.settings.classPrefix + 'title"></div>';
                }
                if(this.settings.showDesc){
                    subHtmlCont3 += '<div class="' + this.settings.classPrefix + 'description"></div>';
                }
                close1 = '<span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>';
                break;
            case 'view7':
                $_next = '<svg class="next_bg" width="22px" height="22px" fill="#999" viewBox="0 0 43 70.8">' +
                    '<path id="XMLID_133_" d="M40,33L9.4,2.4c-1.3-1.3-3.5-1.3-4.7,0L1.9,5.2L29.7,33c1.3,1.4,1.3,3.5,0,4.7L1.9,65.5' +
                    'l2.8,2.8c1.3,1.4,3.5,1.4,4.7,0L40,37.7C41.3,36.5,41.3,34.4,40,33L40,33z"/>' +
                    '<path id="XMLID_132_" d="M4.1,3.2L3.8,3.5l31.4,31.2c0.3,0.4,0.3,1,0,1.3L3.8,67.4l0.3,0.1c0.3,0.4,0.9,0.4,1.3,0' +
                    'L36.9,36c0.3-0.3,0.3-0.9,0-1.3L5.5,3.2C5,2.9,4.4,2.9,4.1,3.2L4.1,3.2z"/>' +
                    '</svg>';
                $_prev = '<svg class="prev_bg" width="22px" height="22px" fill="#999" x="0px" y="0px" viewBox="0 0 43 70.8">' +
                    '<path id="XMLID_133_" d="M2.9,37.8l30.6,30.6c1.3,1.3,3.5,1.3,4.7,0l2.8-2.8L13.2,37.8c-1.3-1.4-1.3-3.5,0-4.7L41,5.3' +
                    'l-2.8-2.8c-1.3-1.4-3.5-1.4-4.7,0L2.9,33.1C1.6,34.3,1.6,36.4,2.9,37.8L2.9,37.8z"/>' +
                    '<path id="XMLID_132_" d="M38.8,67.5l0.3-0.3L7.7,36.1c-0.3-0.4-0.3-1,0-1.3L39.1,3.4l-0.3-0.1c-0.3-0.4-0.9-0.4-1.3,0' +
                    'L6,34.8c-0.3,0.3-0.3,0.9,0,1.3l31.4,31.5C37.9,67.8,38.5,67.8,38.8,67.5L38.8,67.5z"/>' +
                    '</svg>';
                $close_bg = '<svg id="close_hover" class="close_bg" viewBox="0 0 24.3 23.8" style="enable-background:new 0 0 24.3 23.8;" xml:space="preserve">' +
                    '<style type="text/css">.st0{opacity:0.3;}.st2{fill:#FFFFFF;}.st3{fill:#010101;}.st4{fill:#231F20;}</style>' +
                    '<g id="XMLID_248_" class="st0"><g id="XMLID_249_"><g id="XMLID_250_"><defs><rect id="XMLID_137_" x="1.3" y="0.5" width="22.5" height="22.8"/></defs>' +
                    '<clipPath id="XMLID_140_"><use xlink:href="#XMLID_137_"  style="overflow:visible;"/></clipPath><g id="XMLID_251_" class="st1"><g id="XMLID_252_">' +
                    '<path id="XMLID_132_" class="st2" d="M14.4,11.7l9-9c0.5-0.5,0.5-1.3,0-1.9c-0.5-0.5-1.3-0.5-1.9,0l-9,9l-9-9' +
                    'C3,0.3,2.2,0.3,1.7,0.8c-0.5,0.5-0.5,1.3,0,1.9l9,9l-9,9c-0.5,0.5-0.5,1.3,0,1.9C2,22.9,2.3,23,2.6,23s0.7-0.1,0.9-0.4l9-9l9,9' +
                    'c0.3,0.3,0.6,0.4,0.9,0.4s0.7-0.1,0.9-0.4c0.5-0.5,0.5-1.3,0-1.9L14.4,11.7z"/>' +
                    '</g></g></g></g></g><g id="XMLID_142_" class="st0"><line id="XMLID_135_" class="st3" x1="1.9" y1="1.1" x2="23.1" y2="22.3"/>' +
                    '<rect id="XMLID_136_" x="-2.5" y="11.2" transform="matrix(0.7077 0.7066 -0.7066 0.7077 11.918 -5.4355)" class="st4" width="30" height="1"/>' +
                    '</g><g id="XMLID_139_" class="st0"><line id="XMLID_133_" class="st3" x1="23.4" y1="1" x2="2.2" y2="22.2"/>' +
                    '<rect id="XMLID_134_" x="12.3" y="-3.4" transform="matrix(0.7071 0.7071 -0.7071 0.7071 11.9598 -5.6442)" class="st4" width="1" height="30"/>' +
                    '</g></svg>' +
                    '<svg id="close_hover_on" class="close_bg" viewBox="0 0 24.3 23.8" style="enable-background:new 0 0 24.3 23.8;" xml:space="preserve">' +
                    '<style type="text/css">.st0{opacity:0.7;}.st2{fill:#FFFFFF;}.st3{opacity:1;}.st4{fill:#010101;}.st5{fill:#231F20;}</style>' +
                    '<g id="XMLID_248_" class="st0"><g id="XMLID_249_"><g id="XMLID_250_"><defs><rect id="XMLID_137_" x="0.6" y="0.5" width="23" height="23.2"/></defs>' +
                    '<clipPath id="XMLID_140_"><use xlink:href="#XMLID_137_"  style="overflow:visible;"/></clipPath><g id="XMLID_251_" class="st1"><g id="XMLID_252_">' +
                    '<path id="XMLID_132_" class="st2" d="M14.1,12l9.2-9.2c0.5-0.5,0.5-1.4,0-1.9c-0.5-0.5-1.4-0.5-1.9,0L12.1,10L2.9,0.8' +
                    'C2.4,0.3,1.6,0.3,1,0.8C0.5,1.4,0.5,2.2,1,2.8l9.2,9.2l-9.2,9.2c-0.5,0.5-0.5,1.4,0,1.9c0.3,0.3,0.6,0.4,1,0.4s0.7-0.1,1-0.4' +
                    'l9.2-9.2l9.2,9.2c0.3,0.3,0.6,0.4,1,0.4c0.3,0,0.7-0.1,1-0.4c0.5-0.5,0.5-1.4,0-1.9L14.1,12z"/>' +
                    '</g></g></g></g></g><g id="XMLID_142_" class="st3"><line id="XMLID_135_" class="st4" x1="1.3" y1="1.1" x2="22.9" y2="22.7"/>' +
                    '<rect id="XMLID_136_" x="-3.2" y="11.4" transform="matrix(0.7077 0.7066 -0.7066 0.7077 11.9668 -5.0772)" class="st5" width="30.6" height="1"/>' +
                    '</g><g id="XMLID_139_" class="st3"><line id="XMLID_133_" class="st4" x1="23.2" y1="1" x2="1.6" y2="22.7"/>' +
                    '<rect id="XMLID_134_" x="11.9" y="-3.5" transform="matrix(0.7072 0.707 -0.707 0.7072 12.0069 -5.2896)" class="st5" width="1" height="30.6"/>' +
                    '</g></svg>';
                close3 = '<div class="' + this.settings.classPrefix + 'close-bar">' +
                    '<span class="' + this.settings.classPrefix + 'close ' + $object.settings.classPrefix + 'icon">' + $close_bg + '</span>' +
                    '</div>';
                break;
        }

        if (this.settings.arrows && this.$items.length > 1) {
            controls = '<div class="' + this.settings.classPrefix + 'arrows">' +
                '<div class="' + this.settings.classPrefix + 'prev ' + $object.settings.classPrefix + 'icon">' + $_prev + this.settings.prevHtml + '</div>' +
                '<div class="' + this.settings.classPrefix + 'next ' + $object.settings.classPrefix + 'icon">' + $_next + this.settings.nextHtml + '</div>' +
                '</div>';
        }

        if (this.settings.socialSharing && (this.settings.lightboxView !== 'view5' || this.settings.lightboxView !== 'view6')) {
            socialIcons = '<div class="' + this.settings.classPrefix + 'socialIcons"><button class="shareLook">share</button></div>';
        }

        $contInner = (this.settings.lightboxView === 'view5' || this.settings.lightboxView === 'view6') ? '<div class="contInner">' + subHtmlCont3 + '</div>' : '';

        var arrowHE = (this.settings.lightboxView !== 'view2' && this.settings.lightboxView !== 'view3') ? this.settings.arrowsHoverEffect : '',
            $up = '<svg class="rwd-up" width="15px" height="12px" viewBox="0 0 26.5 21" style="enable-background:new 0 0 26.5 21;">' +
                '<g id="Shape_1"><g>' +
                '<path class="st0" d="M23.7,15.3l-9.2-9.2c-0.4-0.4-1-0.4-1.4,0L3.7,15c-0.4,0.4-0.4,1,0,1.4l0.9,0.9l8.6-8.1' +
                'c0.4-0.4,1.1-0.4,1.4,0l8.3,8.3l0.9-0.8C24.1,16.3,24.1,15.7,23.7,15.3z"/>' +
                '</g></g></svg>',
            $down = '<svg class="rwd-down" width="15px" height="12px" viewBox="0 0 26.5 21" style="enable-background:new 0 0 26.5 21;">' +
                '<g id="Shape_1"><g>' +
                '<path class="st0" d="M23.8,6.8l-0.9-0.8l-8.5,8.2c-0.4,0.4-1.1,0.4-1.4,0L4.6,5.8L3.7,6.7C3.3,7,3.3,7.7,3.7,8l9.3,9.1' +
                'c0.4,0.4,1,0.4,1.4,0l9.4-9C24.2,7.8,24.2,7.1,23.8,6.8z"/>' +
                '</g></g></svg>',
            $toggle_bar = (this.settings.lightboxView === 'view7') ? '<div class="toggle_bar">' + $up + $down + '</div>' : '',
            nw = '', nh = '', $lens;

        if(this.settings.lightboxView === 'view7'){
            nw = $object.$items.eq(index).find('img').prop('naturalWidth');
            nh = $object.$items.eq(index).find('img').prop('naturalHeight');
        }

        $lens = (this.settings.lightboxView === 'view7') ? '<div class="tool_bar"><div class="zoom_cont rwd-icon"><div class="zoom_size"></div><div class="img_size">' + nw + 'px &times; ' + nh + 'px</div></div></div>' : '';

        template = '<div class="' + this.settings.classPrefix + 'cont ">' +
            '<div class="rwd-container rwd-' + this.settings.lightboxView + ' rwd-arrows_hover_effect-' + arrowHE + '">' +
            '<div class="cont-inner">' + list + '</div>' +
            $contInner +
            '<div class="' + this.settings.classPrefix + 'toolbar group">' +
            close1 + subHtmlCont2 + $toggle_bar + $lens +
            '</div>' +
            close3 +
            controls +
            '<div class="' + this.settings.classPrefix + 'bar">' +
            close2 + subHtmlCont1 + socialIcons + '</div>' +
            '</div>' +
            '</div>';

        switch($object.settings.openCloseType[0]){
            case 'open_1':
            case 'open_2':
            case 'open_3':
            case 'open_4':
            case 'open_5':
            case 'open_1_r':
            case 'open_2_r':
            case 'open_3_r':
            case 'open_4_r':
            case 'open_5_r':
                setTimeout(function(){
                    $object.$cont.addClass('rwd-visible');
                    $('.rwd-container').addClass($object.settings.openCloseType[0]);
                }, 0);
                break;
            default:
                $('.rwd-container').addClass($object.settings.openCloseType[0]);
                setTimeout(function () {
                    $object.$cont.addClass('rwd-visible');
                }, this.settings.overlayDuration);
                break;
        }

        if ($object.settings.socialSharing) {
            setTimeout(function () {
                $object.socialShare();
            }, 50);
        }

        this.$body.append(template);
        this.$cont = $('.' + $object.settings.classPrefix + 'cont');
        this.$item = this.$cont.find('.' + $object.settings.classPrefix + 'item');

        if (!this.settings.slideAnimation) {
            this.$cont.addClass(this.settings.classPrefix + 'animation');
            this.settings.slideAnimationType = this.settings.classPrefix + 'slide';
        } else {
            this.$cont.addClass(this.settings.classPrefix + 'use');
        }

        $object.calculateDimensions(index);

        this.$item.eq(this.index).addClass(this.settings.classPrefix + 'current');

        if (this.effectsSupport()) {
            this.$cont.addClass(this.settings.classPrefix + 'support');
        } else {
            this.$cont.addClass(this.settings.classPrefix + 'noSupport');
            this.settings.speed = 0;
        }

        this.$cont.addClass(this.settings.slideAnimationType);

        ((this.settings.showAfterLoad) && (this.$cont.addClass(this.settings.classPrefix + 'show-after-load')));

        if (this.effectsSupport()) {
            var $inner = this.$cont.find('.cont-inner');
            $inner.css('transition-timing-function', 'ease');
            $inner.css('transition-duration', this.settings.speed + 'ms');
        }

        switch($object.settings.lightboxView){
            case 'view1':
            case 'view2':
            case 'view3':
                $inner.css({
                    height: 'calc(100% - 92px)',
                    top: '47px'
                });
                break;
            case 'view4':
                $inner.css({
                    height: 'calc(100% - 92px)',
                    top: '45px'
                });
                break;
            case 'view5':
                jQuery('.cont-inner').css({
                    width: '60%'
                });
                break;
            case 'view6':
                jQuery('.cont-inner').css({
                    width: '80%'
                });
                break;
            case 'view7':
                $('.toggle_bar').on('click', function(){
                    $('.rwd-container').toggleClass('rwd_toggle_bar');

                    if (jQuery('.rwd-toolbar').height() === 40) {
                        jQuery('.rwd-toolbar').animate({
                            height: '',
                            top: jQuery('.rwd-toolbar').position().top + 40 + 'px'
                        }, 1000);
                        jQuery('.tool_bar > *').css('display', 'none');
                    } else {
                        jQuery('.rwd-toolbar').animate({
                            height: '40px',
                            top: jQuery('.rwd-toolbar').position().top - 40 + 'px'
                        }, 1000);
                        setTimeout(function () {
                            jQuery('.tool_bar > *').css('display', 'block');
                        }, 1000);
                    }
                });
                break;
        }

        $object.objects.overlay.addClass('in');

        setTimeout(function () {
            $object.$cont.addClass($object.settings.classPrefix + 'visible');
        }, this.settings.overlayDuration);

        if (this.settings.download) {
            $download_bg = '<svg class="download_bg" width="20px" height="20px" stroke="#999" fill="#999"  viewBox="-328 330.3 41.7 41.7" >' +
                '<path class="st0" d="M-296.4,352.1c0.4-0.4,0.4-1.1,0-1.6c-0.4-0.4-1.1-0.4-1.6,0l-8,8V332c0-0.6-0.5-1.1-1.1-1.1c-0.6,0-1.1,0.5-1.1,1.1v26.5l-8-8c-0.4-0.4-1.2-0.4-1.6,0c-0.4,0.4-0.4,1.1,0,1.6l10,10c0.4,0.4,1.1,0.4,1.6,0L-296.4,352.1zM-288.5,359.4c0-0.6,0.5-1.1,1.1-1.1c0.6,0,1.1,0.5,1.1,1.1v10.9c0,0.6-0.5,1.1-1.1,1.1h-39.5c-0.6,0-1.1-0.5-1.1-1.1v-10.9c0-0.6,0.5-1.1,1.1-1.1c0.6,0,1.1,0.5,1.1,1.1v9.8h37.2V359.4z"/>' +
                '</svg>';
            $download_bg_ = '<svg class="download_bg" width="36px" height="34px" stroke="#999" fill="#999" x="0px" y="0px"' +
                'viewBox="0 0 90 90" style="enable-background:new 0 0 90 90;" xml:space="preserve">' +
                '<path id="XMLID_2_" class="st0" d="M61.3,31.8L45.5,47.7c-0.2,0.2-0.5,0.2-0.7,0l-16-15.9c-0.2-0.2-0.2-0.5,0-0.7l2.1-2.1l12.6,12.6' +
                'V7.4c0-0.9,0.7-1.7,1.7-1.7s1.8,0.8,1.8,1.7v34l12.2-12.3l2.1,2.1C61.5,31.3,61.5,31.6,61.3,31.8L61.3,31.8z"/>' +
                '<path id="XMLID_3_" class="st0" d="M25.6,50.7L25.6,50.7h38.7c1.6,0,2.8,1.2,2.8,2.7v1.5c0,1.6-1.2,2.9-2.8,2.9H25.6' +
                'c-1.5,0-2.8-1.3-2.8-2.9v-1.5C22.9,51.9,24.1,50.7,25.6,50.7L25.6,50.7z"/>' +
                '</svg>';
            switch (this.settings.lightboxView) {
                case 'view1':
                default:
                    this.$cont.find('.' + $object.settings.classPrefix + 'toolbar').append('<a id="' + $object.settings.classPrefix + 'download" target="_blank" download class="' + this.settings.classPrefix + 'download ' + $object.settings.classPrefix + 'icon">' + $download_bg + '</a>');
                    break;
                case 'view2':
                    this.$cont.find('.' + $object.settings.classPrefix + 'bar').append('<a id="' + $object.settings.classPrefix + 'download" target="_blank" download class="' + this.settings.classPrefix + 'download ' + $object.settings.classPrefix + 'icon">' + $download_bg + '</a>');
                    break;
                case 'view4':
                    $('<a id="' + $object.settings.classPrefix + 'download" target="_blank" download class="' + this.settings.classPrefix + 'download ' + $object.settings.classPrefix + 'icon">' + $download_bg + '</a>').insertBefore($('.rwd-title'));
                    break;
                case 'view5':
                case 'view6':
                    $('.rwd-toolbar').append('<a id="' + $object.settings.classPrefix + 'download" target="_blank" download class="' + this.settings.classPrefix + 'download ' + $object.settings.classPrefix + 'icon">' + $download_bg_ + '</a>');
                    break;
                case 'view7':
                    $('.tool_bar').append('<a id="' + $object.settings.classPrefix + 'download" target="_blank" download class="' + this.settings.classPrefix + 'download ' + $object.settings.classPrefix + 'icon">' + $download_bg_ + '</a>');
                    break;
            }
        }

        if(this.settings.lightboxView === 'view7' && hugeit_resp_lightbox_obj.hugeit_lightbox_view_info === 'true'){
            var $info = '<svg id="rwd-view7-info" width="20" height="20" stroke="#999" fill="#999" viewBox="0 0 543.906 543.906" style="enable-background:new 0 0 543.906 543.906;">' +
                '<g><path d="M271.953,0C121.759,0,0,121.759,0,271.953s121.759,271.953,271.953,271.953' +
                's271.953-121.759,271.953-271.953S422.148,0,271.953,0z M316.994,76.316c1.055-0.049,2.138-0.06,3.231,0' +
                'c14.724-0.484,27.533,10.622,29.578,24.987c6.576,27.581-22.719,55.228-49.631,44.192' +
                'C268.032,130.576,284.224,77.909,316.994,76.316z M303.739,196.318c20.875-1.327,24.519,22.964,18.014,47.592' +
                'c-12.695,56.583-32.455,111.403-43.175,168.442c5.178,22.523,33.575-3.312,45.721-11.558c10.329-8.213,12.124,2.083,15.637,10.71' +
                'c-25.776,18.058-51.687,36.447-80.395,50.991c-26.972,16.361-49.049-9.072-42.321-37.394' +
                'c11.128-52.841,25.776-104.882,37.736-157.564c3.737-28.468-33.728,0.511-44.872,7.136c-8.985,11.292-13.25,3.051-16.997-7.136' +
                'c29.871-21.816,60.325-48.593,93.313-65.949C293.138,198.238,298.92,196.622,303.739,196.318z"/>' +
                '</g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>';
            $('.tool_bar').append('<span class="info_show_hide rwd-icon">' + $info + '</span>');
        }

        $arrows = $('.rwd-arrows .rwd-next, .rwd-arrows .rwd-prev');
        $next = $('.rwd-arrows .rwd-next');
        $prev = $('.rwd-arrows .rwd-prev');

        var title_text = $('.rwd-title');

        switch (this.settings.titlePos) {
            case 'left':
                title_text.css({'text-align': 'left'});
                break;
            case 'center':
                title_text.css({'text-align': 'center'});
                break;
            case 'right':
                title_text.css({'text-align': 'right'});
                break;
        }

        switch (this.settings.lightboxView) {
            case 'view1':
            default:
                $arrows.css({'top': '50%'});
                $next.css({'right': '20px'});
                $prev.css({'left': '20px'});
                break;
            case 'view2':
                $arrows.css({'bottom': '0'});
                $next.css({'right': '40%'});
                $prev.css({'left': '40%'});
                break;
            case 'view3':
                $arrows.css({'top': '14px', 'z-index': '1090'});
                $next.css({'right': '20px'});
                $prev.css({'right': '55px'});
                title_text.css({'text-align': 'left', 'border-top': '1px solid #999'});
                $('.rwd-close').css({'margin-right': '45%'});
                $('.rwd-overlay, .rwd-toolbar, .rwd-title, .rwd-next, .rwd-prev').css({'background': 'rgba(255, 255, 255, 1)'});
                $('.rwd-title, .shareLook').css({'color': '#999'});
                $('.rwd-toolbar').css({'border-bottom': '1px solid #999'});
                $('.rwd-toolbar .rwd-icon, .rwd-arrows .rwd-icon').addClass('rwd-icon0');
                break;
        }

        this.prevScrollTop = $(window).scrollTop();

        $object.objects.content = $('.rwd-container');

        $object.objects.content.css({
            'width': $object.settings.width,
            'height': $object.settings.height
        });

        if($(window).width() < 768 && this.settings.lightboxView === 'view7'){
            $object.objects.content.css({
                'width': '100%',
                'height': '100%'
            });
        }
        
        var $color, $zoomTop = (document.documentElement.clientHeight - $object.objects.content.height()) / 2;
        switch (this.settings.lightboxView){
            case 'view3':
                $color = 'rgba(255,255,255,.9)';
                break;
            default:
                $color = 'rgba(0,0,0,.9)';
                break;
        }

        if(hugeit_resp_lightbox_obj.hugeit_lightbox_rightclick_protection === 'true'){
            setTimeout(function () {
                $('.rwd-container').bind('contextmenu', function () {
                    return false;
                });
            }, 50);
        }

        if(this.settings.showBorder){
            $('.rwd-container').css({
                border: '2px solid #999'
            });
        }
    };

    Lightbox.prototype.calculateDimensions = function (index) {
        var $object = this, $width;

        $width = $('.' + $object.settings.classPrefix + 'current').height() * 16 / 9;

        if ($width > $object.settings.videoMaxWidth) {
            $width = $object.settings.videoMaxWidth;
        }

        $('.' + $object.settings.classPrefix + 'video-cont ').css({
            'max-width': $width + 'px'
        });

        if(this.settings.lightboxView === 'view7') {
            var $img, $video, $_inner,
                $_width, $_height, $_wrap, $wrapHeight,
                $_top, $top, $left, $_left = 0, $t,
                $length, $k;

            setTimeout(function() {

                $img = document.querySelector('.rwd-item:nth-child(' + (index + 1) + ') img');
                $video = document.querySelector('.rwd-item:nth-child(' + (index + 1) + ') .rwd-video-cont');
                $_inner = document.querySelector('.rwd-item:nth-child(' + (index + 1) + ') .view7_inner');

                if($img && $(window).width() > 768){
                    $k = $object.$item.eq(index).find('img').prop('naturalWidth') / $object.$item.eq(index).find('img').prop('naturalHeight');

                    if ($object.$item.eq(index).find('img').prop('naturalWidth') > $object.$item.eq(index).find('img').prop('naturalHeight') && $k > 2) {
                        if($object.$item.eq(index).find('img').prop('naturalWidth') < 500 ){
                            $object.$item.eq(index).find('img').css({
                                minWidth: '70%'
                            });
                        }
                    } else {
                        if($object.$item.eq(index).find('img').prop('naturalHeight') < 500){
                            $object.$item.eq(index).find('img').css({
                                minHeight: '70%'
                            });
                        }
                    }
                }

                $_width = parseFloat(window.getComputedStyle($img || $video, null).width);
                $_height = parseFloat(window.getComputedStyle($img || $video, null).height);

                switch($img || $video){
                    case $img:
                        $_wrap = document.querySelector('.rwd-item:nth-child(' + (index + 1) + ') .rwd-img-wrap');
                        break;
                    case $video:
                        $_wrap = document.querySelector('.rwd-item:nth-child(' + (index + 1) + ')');
                        break;
                }

                $wrapHeight = parseFloat(window.getComputedStyle($_wrap, null).height);

                $top = ($wrapHeight - $_height) / 2 + $_height - 40;
                $_top = ($wrapHeight - $_height) / 2;
                $left = ($('.rwd-container').width() - (parseFloat(window.getComputedStyle($_inner, null).width) + $_width)) / 2;
                $t = ($object.settings.thumbnail) ? 47 : 0;

                if ($('.rwd-container').width() > 768) {

                    $('.rwd-view7-title').eq(index).width($('.rwd-container').width() * 0.2 - 46);
                    $('.rwd-view7-desc').eq(index).width($('.rwd-container').width() * 0.2 - 36);

                    setTimeout(function(){
                        $('.rwd-view7-desc').height($('.view7_inner').height() - ($object.settings.showTitle ? 120 : 9));
                    }, 200);

                    $_left = ($_inner.classList.contains('is_close')) ? 0 : $('.rwd-container').width() * 0.1;

                    $('.rwd-toolbar').css({
                        width: $_width + 1 + 'px',
                        top: $top + $t + ($('.rwd-container').hasClass('rwd_toggle_bar') ? 40 : 0) + 1 + 'px',
                        left: $left + 'px'
                    });

                    $('.rwd-close-bar').css({
                        top: $_top + $t + 'px',
                        left: $left + $_width - 25 + 'px'
                    });

                    ($img || $video).style.left = -$_left + ($object.settings.showBorder ? $object.settings.borderSize : 0);

                    $('.view7_inner').eq(index).css({
                        top: $_top + 'px',
                        left: $_left + $left + $_width + 'px',
                        height: $_height + 1 + 'px'
                    });

                    $('.view7_share').css({
                        top: $_top + $t + 12 + 'px',
                        right: 2*$_left + $_width + $left + 5 + 'px'
                    });

                    $length = $('.view7_share li').length + 1;

                    $('.view7_share, .view7_share > div').width(($_height - 42) / $length).height(($_height - 42) / $length);
                    $('.view7_share li').width(($_height - 42) / $length).height(($_height - 42) / $length);

                } else {
                    $('.rwd-toolbar').css({
                        width: $(window).width() + 'px',
                        top: '0'
                    });

                    $('.rwd-toolbar .rwd-icon, .view7_share').width($(window).width() * 0.2);

                    $('.view7_share').css({
                        'top': '0',
                        'z-index': '999999999',
                        'left': ($(window).width() * 0.2 - 40) * 0.5 + 'px'
                    });

                    $('.rwd-share-buttons').css({
                        height: $(window).height() + 'px',
                        width: $(window).width() + 'px',
                        left: -($(window).width() * 0.2 - 40) * 0.5 + 'px'
                    });

                    $('.rwd-share-buttons li').css({
                        width: ($(window).width() - ($object.settings.showBorder ? 8 : 0)) * 0.2 + 'px',
                        maxWidth: 'none'
                    });

                    $('.rwd-share-buttons li a').css({
                        marginLeft: $(window).width() * 0.1 - 20 + 'px'
                    });

                    $('.rwd-close-bar').css({
                        top: $_top + $t + 5 + 'px',
                        left: ($(window).width() - $_width) / 2 + $_width - 30 + 'px'
                    });
                }
            }, 0);
        }
    };

    Lightbox.prototype.innerOpenClose = function(){
        var $object = this;

        $('.info_show_hide').off('click');

        $('.info_show_hide').on('click', function(){
            var $view7_inner = $('.rwd-item.rwd-current').find('.view7_inner'),
                $toolbar = $('.rwd-toolbar'),
                $closebar = $('.rwd-close-bar'),
                $imgcont = $('.rwd-item.rwd-current').find('img'),
                $videoCont = $('.rwd-item.rwd-current').find('.rwd-video-cont '),
                $zoomcont = $('.zoomContainer'),
                $view7_share = $('.view7_share');

            if($('.rwd-container').width() > 768) {
                if ($('.rwd-item.rwd-current').hasClass('isImg')) {
                    if (!$view7_inner.hasClass('is_close')) {
                        $view7_inner.addClass('is_close');

                        $view7_inner.animate({
                            width: '0',
                            left: parseFloat($view7_inner.css('left')) + 'px'
                        }, 1000);

                        $toolbar.animate({
                            width: parseFloat($toolbar.css('width')) + 1 + 'px',
                            left: parseFloat($toolbar.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $closebar.animate({
                            left: parseFloat($closebar.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $imgcont.animate({
                            left: parseFloat($imgcont.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $zoomcont.animate({
                            left: parseFloat($imgcont.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $view7_share.animate({
                            left: parseFloat($view7_share.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);
                    } else {
                        $view7_inner.removeClass('is_close');

                        $view7_inner.animate({
                            width: $('.rwd-container').width() * 0.2 + 'px',
                            left: parseFloat($view7_inner.css('left')) + 'px'
                        }, 1000);

                        $toolbar.animate({
                            width: parseFloat($toolbar.css('width')) - 1 + 'px',
                            left: parseFloat($toolbar.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $closebar.animate({
                            left: parseFloat($closebar.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $imgcont.animate({
                            left: parseFloat($imgcont.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $zoomcont.animate({
                            left: parseFloat($imgcont.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $view7_share.animate({
                            left: parseFloat($view7_share.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);
                    }
                } else if ($('.rwd-item.rwd-current').hasClass('isVideo')) {
                    if (!$view7_inner.hasClass('is_close')) {
                        $view7_inner.addClass('is_close');

                        $view7_inner.animate({
                            width: '0',
                            left: parseFloat($view7_inner.css('left')) + 'px'
                        }, 1000);

                        $toolbar.animate({
                            left: parseFloat($toolbar.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 0.5 + 'px'
                        }, 1000);

                        $closebar.animate({
                            left: parseFloat($closebar.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $videoCont.animate({
                            left: parseFloat($videoCont.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);

                        $view7_share.animate({
                            left: parseFloat($view7_share.css('left')) + parseFloat($view7_inner.css('width')) / 2 + 'px'
                        }, 1000);
                    } else {
                        $view7_inner.removeClass('is_close');

                        $view7_inner.animate({
                            width: $('.rwd-container').width() * 0.2 + 'px',
                            left: parseFloat($view7_inner.css('left')) + 'px'
                        }, 1000);

                        $toolbar.animate({
                            left: parseFloat($toolbar.css('left')) - $('.rwd-container').width() * 0.1 - 0.5 + 'px'
                        }, 1000);

                        $closebar.animate({
                            left: parseFloat($closebar.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $videoCont.animate({
                            left: parseFloat($videoCont.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);

                        $view7_share.animate({
                            left: parseFloat($view7_share.css('left')) - $('.rwd-container').width() * 0.1 + 'px'
                        }, 1000);
                    }
                }
            } else {
                if (!$view7_inner.hasClass('is_open')) {
                    $view7_inner.addClass('is_open');

                    if($('.rwd-share-buttons').css('display') === 'block'){
                        $('.rwd-share-buttons').css({
                            'display': 'none'
                        });
                    }

                    $('.rwd-close-bar,.rwd-arrows .rwd-next, .rwd-arrows .rwd-prev').css({
                        zIndex: '1'
                    });

                    if(!$object.settings.thumbnail){
                        $view7_inner.css({
                            top: '40px'
                        });
                    }

                    if($object.settings.thumbnail && hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs_position === '1'){
                        $view7_inner.css({
                            top: -(+hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs_height) + 'px'
                        });
                    }

                    $view7_inner.animate({
                        height: $(window).height() + 'px'
                    }, 1000);
                } else {
                    $view7_inner.removeClass('is_open');

                    $('.rwd-close-bar,.rwd-arrows .rwd-next, .rwd-arrows .rwd-prev').animate({
                        zIndex: '999999999'
                    }, 700);

                    $view7_inner.animate({
                        height: '0'
                    }, 1000);
                }
            }
        });
    };

    Lightbox.prototype.effectsSupport = function () {
        var transition, root, support;
        support = function () {
            transition = ['transition', 'MozTransition', 'WebkitTransition', 'OTransition', 'msTransition', 'KhtmlTransition'];
            root = document.documentElement;
            for (var i = 0; i < transition.length; i++) {
                if (transition[i] in root.style) {
                    return transition[i] in root.style;
                }
            }
        };

        return support();
    };

    Lightbox.prototype.isVideo = function (src, index) {
        var youtube, vimeo, dailymotion;

        youtube = src.match(/\/\/(?:www\.)?youtu(?:\.be|be\.com)\/(?:watch\?v=|embed\/)?([a-z0-9\-\_\%]+)/i);
        vimeo = src.match(/\/\/(?:www\.)?vimeo.com\/([0-9a-z\-_]+)/i);
        dailymotion = src.match(/^.+dailymotion.com\/(video|hub)\/([^_]+)[^#]*(#video=([^_&]+))?/);

        if (youtube) {
            return {
                youtube: youtube
            };
        } else if (vimeo) {
            return {
                vimeo: vimeo
            };
        } else if (dailymotion) {
            return {
                dailymotion: dailymotion
            };
        }
    };

    Lightbox.prototype.counter = function () {
        if (this.settings.showCounter) {
            switch (this.settings.lightboxView) {
                case 'view1':
                default:
                    $('.' + this.settings.classPrefix + 'toolbar').append(this.objects.counter = $('<div id="' + this.settings.idPrefix + 'counter"></div>'));
                    $('#rwd-counter').css({'padding-left': '23px'});
                    break;
                case 'view2':
                case 'view4':
                case 'view7':
                    $('.' + this.settings.classPrefix + 'bar').append('<div class="barCont"></div>').append(this.objects.counter = $('<div id="' + this.settings.idPrefix + 'counter"></div>'));
                    break;
                case 'view5':
                case 'view6':
                    $('.contInner').append(this.objects.counter = $('<div id="' + this.settings.idPrefix + 'counter"></div>'));
                    break;
            }

            this.objects.counter.append(
                this.objects.current = $('<div>' + this.settings.sequence_info + ' <span id="' + this.settings.idPrefix + 'counter-current">' + (parseInt(this.index, 10) + 1) + '</span> ' +
                    this.settings.sequenceInfo + ' <span id="' + this.settings.idPrefix + 'counter-all">' + this.$items.length + '</span></div>')
            );
        }
    };

    Lightbox.prototype.setTitle = function (index) {
        var $object = this, $title, $currentElement;

        $currentElement = this.$items.eq(index);
        $title = $currentElement.find('img').attr('alt') ||
            $currentElement.find('img').attr('title') ||
            this.settings.defaultTitle || '';

        this.$cont.find('.' + this.settings.classPrefix + 'title').html('<div class="rwd-title-text">' + $title + '</div>');

        (($object.settings.lightboxView === 'view2') && $('.rwd-title-text').css({'width': '100%'}));

        if ($object.settings.lightboxView !== 'view1' && $object.settings.lightboxView !== 'view3' && $object.settings.lightboxView !== 'view4') {
            ($title === '' && $object.settings.socialSharing) ?
                this.$cont.find('.' + this.settings.classPrefix + 'title').hide() :
                this.$cont.find('.' + this.settings.classPrefix + 'title').show();
        }

        if($object.settings.lightboxView === 'view7'){
            if(this.settings.showTitle){
                $('.rwd-view7-title').eq(index).html($title);
            }
        }
    };

    Lightbox.prototype.setDescription = function (index) {
        var $object = this, $description, $currentElement;

        $currentElement = this.$items.eq(index);
        $description = $currentElement.find('img').attr('data-description') || '';

        this.$cont.find('.' + this.settings.classPrefix + 'description').html('<div class="rwd-description-text" title="' + $description + '">' + $description + '</div>');

        if($object.settings.lightboxView === 'view7'){
            if(this.settings.showDesc){
                $('.rwd-view7-desc').eq(index).html($description);
                $('.rwd-view7-desc').eq(index).attr('title', $description);
            }
        }
    };

    Lightbox.prototype.preload = function (index) {
        for (var i = 1; i <= this.settings.preload; i++) {
            if (i >= this.$items.length - index) {
                break;
            }

            this.loadContent(index + i, false, 0);
        }

        for (var j = 1; j <= this.settings.preload; j++) {
            if (index - j < 0) {
                break;
            }

            this.loadContent(index - j, false, 0);
        }
    };

    Lightbox.prototype.socialShare = function () {
        var $object = this, shareButtons;

        if(this.settings.lightboxView !== 'view7'){
            shareButtons = '<ul class="rwd-share-buttons">';
            shareButtons += $object.settings.share.facebookButton ? '<li><a title="Facebook" id="rwd-share-facebook" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.twitterButton ? '<li><a title="Twitter" id="rwd-share-twitter" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.googleplusButton ? '<li><a title="Google Plus" id="rwd-share-googleplus" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.pinterestButton ? '<li><a title="Pinterest" id="rwd-share-pinterest" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.linkedinButton ? '<li><a title="Linkedin" id="rwd-share-linkedin" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.tumblrButton ? '<li><a title="Tumblr" id="rwd-share-tumblr" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.redditButton ? '<li><a title="Reddit" id="rwd-share-reddit" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.bufferButton ? '<li><a title="Buffer" id="rwd-share-buffer" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.diggButton ? '<li><a title="Digg" id="rwd-share-digg" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.vkButton ? '<li><a title="VK" id="rwd-share-vk" target="_blank"></a></li>' : '';
            shareButtons += $object.settings.share.yummlyButton ? '<li><a title="Yummly" id="rwd-share-yummly" target="_blank"></a></li>' : '';
            shareButtons += '</ul>';
        } else {
            var $fb = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"' +
                'style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;</style>' +
                '<g id="Shape_3"> <g id="XMLID_84_"> <path id="XMLID_85_" class="st0"' +
                'd="M16.8,12.8V11c0-0.9,0.6-1.1,1.1-1.1c0.4,0,2.7,0,2.7,0V6l-3.7,0c-4.1,0-5.1,2.9-5.1,4.8v2.1H9.4v4.6h2.4c0,5.2,0,11.4,0,11.4h4.8c0,0,0-6.3,0-11.4h3.6l0.2-1.8l0.3-2.8H16.8z"></path>' +
                '</g> </g></svg>';

            var $gp = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"' +
                'style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill: #FFFFFF;}</style>' +
                '<g id="Shape_4"> <g id="XMLID_99_"> <path id="XMLID_100_" class="st0"' +
                'd="M23.4,16.1v-0.4c0-0.6,0-1.1,0-1.7c0-0.1-0.1-0.3-0.2-0.3c-0.5,0-1,0-1.6,0v2.3h-2.4c0,0.2,0,0.3,0,0.4c0,0.4,0,0.8,0.1,1.3c0.4,0,0.8,0,1.3,0c0.4,0,0.7,0,1.1,0v2.3h1.7v-2.3h2.4v-1.7H23.4z M17.1,15.9c-1.5,0-3,0-4.5,0c-0.4,0-0.9,0-1.3,0c-0.1,0-0.2,0.1-0.2,0.2c0,0.8,0,1.7,0,2.5h3.7c0,0.2-0.1,0.3-0.1,0.4c-0.6,1.6-2.3,2.4-4.1,2.1c-2.3-0.4-3.7-2.4-3.3-4.6c0.1-0.3,0.2-0.6,0.3-0.9c0.8-1.6,2.6-2.6,4.5-2.2c0.7,0.1,1.2,0.5,1.7,0.9c0.7-0.7,1.3-1.3,2-2c0,0-0.1-0.1-0.2-0.1c-1.6-1.3-3.5-1.8-5.5-1.5c-3,0.5-5.1,2.8-5.6,5.4c-0.2,1.1-0.1,2.3,0.4,3.5c1.1,2.8,4.1,4.6,7.2,4.2c1.5-0.2,2.9-0.7,4-1.8c1.6-1.6,2-3.6,1.7-5.7C17.6,15.9,17.4,15.9,17.1,15.9z"></path>' +
                '</g> </g></svg>';

            var $twitter = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"' +
                'style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;}</style> <g id="Shape_5"> <g id="XMLID_84_"> <path id="XMLID_85_" class="st0"' +
                'd="M27,11.9c-0.8,0.4-1.6,0.6-2.5,0.7c0.9-0.6,1.6-1.5,1.9-2.5c-0.9,0.5-1.8,0.9-2.8,1.1c-0.8-0.9-2-1.4-3.2-1.4c-2.4,0-4.4,2-4.4,4.6c0,0.4,0,0.7,0.1,1c-3.7-0.2-6.9-2-9.1-4.8c-0.4,0.7-0.6,1.5-0.6,2.3c0,1.6,0.8,3,2,3.8c-0.7,0-1.4-0.2-2-0.6v0.1c0,0.6,0.1,1.1,0.3,1.7c0.5,1.4,1.8,2.5,3.2,2.8c-0.4,0.1-0.8,0.2-1.2,0.2c-0.3,0-0.6,0-0.8-0.1c0.6,1.8,2.2,3.1,4.1,3.2c-1.5,1.2-3.4,2-5.5,2c-0.4,0-0.7,0-1.1-0.1c2,1.3,4.3,2,6.8,2c6.9,0,11.2-5,12.3-10.2c0.2-0.9,0.3-1.8,0.3-2.8c0-0.2,0-0.4,0-0.6C25.6,13.6,26.4,12.8,27,11.9z"></path>' +
                '</g> </g></svg>';

            var $vk = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"' +
                'style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;}</style> <g id="Shape_12"> <g id="XMLID_86_"> <path id="XMLID_87_" class="st0"' +
                'd="M26,23.7c-0.4-0.7-1.1-1.6-2.2-2.7l0,0l0,0l0,0h0c-0.5-0.5-0.8-0.8-0.9-1c-0.2-0.3-0.3-0.7-0.2-1c0.1-0.3,0.4-0.8,1.1-1.6c0.3-0.4,0.6-0.8,0.8-1c1.3-1.9,1.9-3.1,1.8-3.6l-0.1-0.1c0-0.1-0.2-0.1-0.4-0.2c-0.2-0.1-0.4-0.1-0.7,0l-3.4,0c-0.1,0-0.1,0-0.2,0c-0.1,0-0.2,0-0.2,0l-0.1,0l0,0c0,0-0.1,0.1-0.1,0.1c0,0.1-0.1,0.1-0.1,0.2c-0.4,1-0.8,1.9-1.3,2.7c-0.3,0.5-0.6,0.9-0.8,1.3c-0.2,0.4-0.4,0.6-0.6,0.8c-0.2,0.2-0.3,0.3-0.4,0.4c-0.1,0.1-0.2,0.1-0.3,0.1c-0.1,0-0.1,0-0.2,0c-0.1-0.1-0.2-0.2-0.3-0.3c-0.1-0.1-0.1-0.3-0.1-0.5c0-0.2,0-0.4,0-0.5c0-0.1,0-0.3,0-0.6c0-0.3,0-0.4,0-0.5c0-0.3,0-0.6,0-1c0-0.4,0-0.6,0-0.9s0-0.4,0-0.7c0-0.2,0-0.4,0-0.6c0-0.1-0.1-0.3-0.1-0.4c-0.1-0.1-0.1-0.2-0.2-0.3c-0.1-0.1-0.2-0.1-0.4-0.2c-0.4-0.1-0.9-0.1-1.6-0.2c-1.5,0-2.4,0.1-2.8,0.3c-0.2,0.1-0.3,0.2-0.4,0.4c-0.1,0.2-0.2,0.3-0.1,0.3c0.5,0.1,0.8,0.2,1,0.5l0.1,0.1c0.1,0.1,0.1,0.3,0.2,0.6c0.1,0.3,0.1,0.6,0.1,0.9c0,0.6,0,1.1,0,1.5c0,0.4-0.1,0.7-0.1,1c0,0.2-0.1,0.4-0.2,0.6c-0.1,0.1-0.1,0.2-0.1,0.3c0,0,0,0.1-0.1,0.1c-0.1,0-0.2,0.1-0.3,0.1c-0.1,0-0.2-0.1-0.4-0.2c-0.2-0.1-0.3-0.3-0.5-0.5c-0.2-0.2-0.4-0.5-0.6-0.8c-0.2-0.4-0.4-0.8-0.7-1.3l-0.2-0.4c-0.1-0.2-0.3-0.6-0.5-1c-0.2-0.4-0.4-0.9-0.5-1.3c-0.1-0.2-0.2-0.3-0.3-0.4l-0.1,0c0,0-0.1-0.1-0.2-0.1c-0.1,0-0.2-0.1-0.3-0.1l-3.2,0c-0.3,0-0.5,0.1-0.7,0.2l0,0.1c0,0,0,0.1,0,0.2c0,0.1,0,0.2,0.1,0.3c0.5,1.1,1,2.2,1.5,3.3c0.5,1.1,1,1.9,1.4,2.6C7.2,19.8,7.6,20.4,8,21c0.4,0.6,0.7,0.9,0.8,1.1c0.1,0.2,0.2,0.3,0.3,0.4l0.3,0.3c0.2,0.2,0.5,0.4,0.8,0.7c0.4,0.3,0.8,0.5,1.2,0.8c0.4,0.3,1,0.5,1.5,0.6c0.6,0.2,1.2,0.2,1.7,0.2h1.3c0.3,0,0.5-0.1,0.6-0.3l0-0.1c0,0,0.1-0.1,0.1-0.2c0-0.1,0-0.2,0-0.3c0-0.3,0-0.7,0.1-0.9c0.1-0.3,0.1-0.5,0.2-0.6c0.1-0.1,0.2-0.3,0.3-0.4c0.1-0.1,0.2-0.2,0.2-0.2c0,0,0.1,0,0.1,0c0.2-0.1,0.4,0,0.7,0.2c0.3,0.2,0.5,0.4,0.7,0.7c0.2,0.3,0.5,0.6,0.8,0.9c0.3,0.3,0.6,0.6,0.8,0.8l0.2,0.1c0.2,0.1,0.4,0.2,0.6,0.3c0.2,0.1,0.5,0.1,0.7,0.1l3,0c0.3,0,0.5-0.1,0.7-0.2c0.2-0.1,0.3-0.2,0.3-0.3c0-0.1,0-0.3,0-0.4c0-0.2-0.1-0.3-0.1-0.3C26.1,23.8,26.1,23.8,26,23.7z"></path> </g> </g></svg>';

            var $pinterest = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;}</style> <g id="Shape_7"> <g id="XMLID_84_"> <path id="XMLID_85_" class="st0"d="M16.6,4.4c-6.9,0-10.4,5-10.4,9.2c0,0.8,0.1,1.5,0.3,2.2c0.4,1.6,1.3,2.8,2.7,3.4c0.3,0.1,0.6,0,0.7-0.4c0.1-0.3,0.2-0.9,0.3-1.2c0.1-0.4,0.1-0.5-0.2-0.8c-0.4-0.4-0.6-0.9-0.8-1.5C9.1,15,9,14.5,9,14c0-3.7,2.8-7.1,7.2-7.1c3.9,0,6.1,2.4,6.1,5.6c0,1-0.1,1.9-0.3,2.7c-0.6,3-2.2,5.1-4.4,5.1c-1.5,0-2.7-1.3-2.3-2.8c0.2-0.9,0.5-1.8,0.8-2.7c0.3-1,0.5-1.8,0.5-2.5c0-1.2-0.6-2.2-2-2.2c-1.6,0-2.8,1.6-2.8,3.8c0,0.4,0,0.8,0.1,1.1c0.1,0.8,0.4,1.2,0.4,1.2s-1.6,6.8-1.9,8c-0.6,2.4-0.1,5.3,0,5.6c0,0.2,0.2,0.2,0.3,0.1c0.1-0.2,2-2.5,2.6-4.8c0.2-0.7,1-4,1-4c0.5,1,2,1.8,3.6,1.8c3.8,0,6.6-2.9,7.5-7c0.2-1,0.3-2,0.3-3C25.8,8.5,22.2,4.4,16.6,4.4z"></path> </g> </g></svg>';

            var $linkedin = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5"style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill: #FFFFFF;}</style> <g id="Shape_8"> <g id="XMLID_107_"> <path id="XMLID_108_" class="st0"d="M9.8,12.5l-4.6,0l0,3.5l0,9.8l4.6,0l0-10.5L9.8,12.5z M7.5,10.5c1.3,0,2.3-1,2.3-2.3c0-1.3-1-2.3-2.3-2.3c-1.3,0-2.3,1-2.3,2.3C5.2,9.5,6.2,10.5,7.5,10.5z M24.8,18.5L24.8,18.5c0-0.7,0-1.3-0.1-1.9c-0.3-2.5-1.4-4.1-4.8-4.1c-2,0-3.3,0.7-3.8,1.8l-0.1,0l0-1.8l-3.6,0l0,2.5l0,10.7l3.8,0l0-6.6c0-1.7,0.3-3.4,2.4-3.4c2.1,0,2.3,2,2.3,3.5l0,6.5l3.9,0L24.8,18.5z"></path> </g> </g></svg>';

            var $tumblr = ' <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" viewBox="0 0 30.101 30.102" style="enable-background:new 0 0 30.101 30.102;" xml:space="preserve"><g> <path d="M24.313,28.246c0,0.154-0.041,0.23-0.187,0.301c-2.326,1.117-4.771,1.661-7.354,1.537 c-1.592-0.077-3.122-0.395-4.528-1.182c-0.612-0.344-1.158-0.772-1.653-1.271c-0.884-0.884-1.327-1.962-1.421-3.192c-0.043-0.57-0.073-1.144-0.074-1.715c-0.007-3.347-0.003-6.695-0.003-10.042c0-0.101,0-0.202,0-0.342c-1.113,0-2.19,0-3.287,0c-0.005-0.098-0.013-0.174-0.013-0.251c0-1.277,0.002-2.556-0.004-3.833c0-0.146,0.035-0.222,0.183-0.273 c3.151-1.107,5.005-3.342,5.688-6.583c0.049-0.229,0.094-0.461,0.133-0.692C11.832,0.478,11.863,0.245,11.9,0 c1.414,0,2.822,0,4.26,0c0,2.526,0,5.053,0,7.602c2.393,0,4.756,0,7.141,0c0,1.585,0,3.147,0,4.732c-2.365,0-4.729,0-7.108,0c-0.013,0.079-0.021,0.139-0.021,0.198c0.002,2.837-0.004,5.677,0.012,8.515c0.004,0.617,0.07,1.234,0.139,1.852 c0.082,0.741,0.475,1.295,1.107,1.687c0.938,0.577,1.963,0.698,3.033,0.592c1.297-0.13,2.475-0.601,3.575-1.278  c0.076-0.048,0.156-0.094,0.271-0.162c0,0.092,0,0.151,0,0.211C24.308,25.377,24.306,26.811,24.313,28.246z"fill="#FFFFFF"></path> </g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g></svg>';

            var $reddit = ' <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5" style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;}</style> <g id="Shape_25"> <g id="XMLID_146_"> <path id="XMLID_147_" class="st0"d="M26.7,16.2L26.7,16.2L26.7,16.2L26.7,16.2c-0.1-0.7-0.3-1.4-0.7-1.9c-0.4-0.5-1-0.9-1.6-1.1c0,0,0,0,0,0l0,0h0c-0.3-0.1-0.5-0.1-0.8-0.1c-0.6,0-1.2,0.2-1.7,0.6c-0.1,0.1-0.3,0.2-0.4,0.3c-0.1,0-0.1-0.1-0.1-0.1l0,0h0c-1.7-1.1-3.6-1.6-5.4-1.7c0-0.9,0.1-1.7,0.4-2.4v0c0.2-0.5,0.7-0.8,1.2-0.9h0c0.1,0,0.2,0,0.3,0c0.7,0,1.3,0.3,1.9,0.6c0,0,0,0.1,0,0.1c0,0.8,0.3,1.5,0.7,2c0.5,0.5,1.1,0.9,1.7,0.9c0.1,0,0.2,0,0.3,0c0.7,0,1.3-0.3,1.8-0.8c0.5-0.5,0.9-1.2,0.9-2v0l0,0c0-0.1,0-0.2,0-0.3c0-0.6-0.2-1.2-0.5-1.7s-0.7-0.9-1.2-1.1c0,0,0,0,0,0c0,0,0,0,0,0v0c-0.3-0.2-0.7-0.2-1.1-0.2c-0.3,0-0.7,0.1-1,0.2l0,0l0,0c-0.6,0.2-1,0.7-1.4,1.2c-0.7-0.3-1.5-0.5-2.4-0.5c-0.3,0-0.6,0-0.9,0.1h0l0,0C16,7.5,15.4,8.2,15,9c-0.5,1-0.6,2.1-0.6,3.1c-1.9,0.1-3.9,0.6-5.6,1.7c-0.1,0-0.2,0.1-0.3,0.2c-0.3-0.2-0.6-0.4-0.9-0.6h0l0,0c-0.4-0.2-0.8-0.2-1.1-0.2c-0.1,0-0.2,0-0.2,0h0c-0.8,0-1.5,0.4-2,1c-0.5,0.6-0.9,1.4-0.9,2.3v0c0,0.6,0.2,1.2,0.5,1.7c0.3,0.5,0.7,0.9,1.1,1.1c0,0.3,0,0.5,0,0.8c0,1.1,0.3,2.3,0.8,3.2c1,1.8,2.6,3,4.2,3.8h0v0c1.6,0.7,3.3,1.1,5.1,1.1c1.2,0,2.3-0.2,3.4-0.5l0,0c2.1-0.6,4.2-1.8,5.6-3.8h0v0c0.7-1.1,1.2-2.4,1.2-3.8c0-0.3,0-0.5,0-0.8c0.5-0.3,0.8-0.7,1.1-1.2c0.3-0.5,0.5-1.1,0.5-1.8C26.7,16.3,26.7,16.3,26.7,16.2z M21.4,8.4c0.2-0.2,0.4-0.4,0.7-0.5l0,0h0c0.1,0,0.2-0.1,0.3-0.1c0.3,0,0.7,0.2,0.9,0.4c0.2,0.3,0.4,0.6,0.4,1c0,0,0,0.1,0,0.1v0c0,0.4-0.2,0.7-0.4,1c-0.2,0.3-0.6,0.4-0.9,0.4c0,0,0,0-0.1,0h0c-0.3,0-0.6-0.2-0.9-0.4c-0.2-0.3-0.4-0.6-0.4-1v0c0,0,0-0.1,0-0.1C21.1,9,21.3,8.7,21.4,8.4z M5.4,17.7c-0.1-0.1-0.3-0.3-0.3-0.5c-0.1-0.2-0.2-0.5-0.2-0.8c0,0,0-0.1,0-0.1l0,0v0c0-0.4,0.2-0.8,0.5-1.1c0.3-0.3,0.7-0.5,1-0.5l0,0h0c0,0,0.1,0,0.1,0c0.3,0,0.5,0.1,0.8,0.2C6.5,15.7,5.8,16.6,5.4,17.7z M23.6,20.9L23.6,20.9c-0.1,0.7-0.4,1.3-0.8,1.9c-0.4,0.6-0.8,1.1-1.3,1.5v0c-1.6,1.3-3.5,2-5.5,2.1c-0.3,0-0.7,0-1,0c-1.9,0-3.9-0.5-5.6-1.5c-0.1-0.1-0.3-0.2-0.4-0.3c-0.6-0.4-1.1-0.9-1.6-1.5c-0.5-0.6-0.8-1.3-1-2l0,0c-0.1-0.3-0.1-0.7-0.1-1c0-0.9,0.3-1.9,0.8-2.7v0c1-1.5,2.5-2.5,4.1-3.1c1.2-0.4,2.5-0.6,3.8-0.6c0.9,0,1.9,0.1,2.8,0.4c0.3,0.1,0.6,0.2,0.9,0.3l0,0c1.6,0.6,3.1,1.6,4.1,3.1c0.5,0.8,0.8,1.7,0.8,2.7C23.7,20.4,23.6,20.7,23.6,20.9z M25,17.2c-0.1,0.2-0.2,0.4-0.4,0.5c-0.4-1.1-1.1-2-1.9-2.7c0.2-0.1,0.5-0.2,0.8-0.2c0.3,0,0.6,0.1,0.9,0.2c0.3,0.2,0.5,0.4,0.6,0.7l0,0l0,0c0.1,0.2,0.1,0.4,0.1,0.7C25.2,16.7,25.1,16.9,25,17.2z M10.9,20.3C10.9,20.3,10.9,20.3,10.9,20.3c0.2,0.1,0.3,0.1,0.5,0.1c0.4,0,0.7-0.2,1-0.5c0.3-0.3,0.5-0.7,0.5-1.1v0c0,0,0,0,0-0.1c0-0.5-0.2-0.9-0.5-1.1c-0.3-0.3-0.6-0.5-1-0.5c-0.1,0-0.2,0-0.3,0c0,0,0,0,0,0h0c-0.5,0.1-0.9,0.5-1.1,1.1c0,0,0,0,0,0c0,0.2-0.1,0.3-0.1,0.5c0,0.4,0.1,0.7,0.3,1C10.3,20,10.6,20.2,10.9,20.3C10.9,20.3,10.9,20.3,10.9,20.3z M19.1,22.4c-0.1-0.1-0.3-0.1-0.4-0.1c-0.1,0-0.2,0-0.4,0.1c-1,0.7-2.2,1.1-3.5,1.1c-0.9,0-1.8-0.2-2.6-0.7l0,0l0,0c-0.1-0.1-0.3-0.2-0.4-0.3c-0.1-0.1-0.2-0.1-0.2-0.2c-0.1,0-0.2-0.1-0.3-0.1c-0.1,0-0.2,0-0.3,0.1c0,0,0,0,0,0c-0.1,0.1-0.3,0.2-0.3,0.3c-0.1,0.1-0.1,0.3-0.1,0.5c0,0.1,0,0.3,0.1,0.4c0.1,0.1,0.1,0.2,0.3,0.3c1.2,0.9,2.5,1.3,3.9,1.3c1.2,0,2.5-0.3,3.6-0.9l0,0l0,0c0.1-0.1,0.3-0.2,0.5-0.3c0.1-0.1,0.2-0.1,0.3-0.2c0.1-0.1,0.1-0.2,0.2-0.4c0-0.1,0-0.1,0-0.2c0-0.1,0-0.2-0.1-0.3C19.3,22.7,19.2,22.5,19.1,22.4z M20,18.2C20,18.2,20,18.2,20,18.2C20,18.2,20,18.2,20,18.2c-0.1-0.4-0.3-0.6-0.6-0.8c-0.3-0.2-0.6-0.3-0.9-0.3c-0.2,0-0.3,0-0.5,0.1c0,0,0,0,0,0c0,0,0,0,0,0c-0.3,0.1-0.5,0.3-0.7,0.6c-0.2,0.3-0.3,0.6-0.3,0.9c0,0.1,0,0.2,0,0.3h0c0,0,0,0,0,0v0c0.1,0.4,0.3,0.7,0.5,1c0.3,0.2,0.6,0.4,0.9,0.4c0.1,0,0.3,0,0.4-0.1h0c0,0,0,0,0,0h0c0.3-0.1,0.6-0.3,0.7-0.6c0.2-0.3,0.3-0.6,0.3-1C20.1,18.6,20.1,18.4,20,18.2L20,18.2z"></path> </g> </g></svg>';

            var $digg = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5" style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #FFFFFF;}</style> <g id="Shape_11"> <g id="XMLID_158_"> <path id="XMLID_159_" class="st0" d="M20.2,14.4v8.1h4v1.8h-4v2.3h6V14.4H20.2z M24.2,20.5h-1.7v-4h1.7V20.5z M13.8,22.5h3.7v1.8h-3.7v2.3h6V14.4h-6V22.5z M16,16.4h1.5v4H16V16.4z M7.8,14.4h-4v8.1H10V10.4H7.8V14.4z M7.8,20.5H6v-4h1.7V20.5z M10.5,22.5h2.2v-8.1h-2.2V22.5z M10.5,12.7h2.2v-2.3h-2.2V12.7z"></path> </g> </g></svg>';

            var $buffer = '<svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5" style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><style type="text/css">.st0 {fill: #FFFFFF;}</style> <g id="Shape_13"> <g id="XMLID_104_"> <path id="XMLID_105_" class="st0"d="M4.5,13l10.3,5.2c0.1,0.1,0.3,0.1,0.4,0L25.5,13c0.3-0.2,0.3-0.7,0-0.8L15.2,7C15.1,7,14.9,7,14.8,7L4.5,12.2C4.1,12.3,4.1,12.8,4.5,13z M25.5,18.4l-2.2-1.1l-6.9,3.5C16,21,15.5,21.1,15,21.1c-0.5,0-1-0.1-1.4-0.3l-6.9-3.5l-2.2,1.1c-0.3,0.2-0.3,0.7,0,0.8l10.3,5.2c0.1,0.1,0.3,0.1,0.4,0l10.3-5.2C25.8,19.1,25.8,18.6,25.5,18.4z M25.5,24.7l-2.2-1.1l-6.9,3.5c-0.4,0.2-0.9,0.3-1.4,0.3c-0.5,0-1-0.1-1.4-0.3l-6.9-3.5l-2.2,1.1c-0.3,0.2-0.3,0.7,0,0.8l10.3,5.2c0.1,0.1,0.3,0.1,0.4,0l10.3-5.2C25.8,25.3,25.8,24.9,25.5,24.7z"></path> </g> </g></svg>';

            var $yummly = ' <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 30 34.5" style="enable-background:new 0 0 30 34.5;" xml:space="preserve"><image style="overflow:visible;" width="36" height="14" id="Layer_3_copy_xA0_Image" xlink:href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACYAAAAPCAYAAACInr1QAAAACXBIWXMAAA4mAAAOJgGi7yX8AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABWxJREFUeNrMVWtsVEUUnpl7797t3e3utt0u22wJtVisjYCIhhITQiKJMUYMUWIiqRET0JjoP01QQkyIf0yIxhh/ScIvfEREUYogCAUqfWHf25YtfWxLu+1uu6/7fsx47jZpICb8QRJvsrtzZ86eOd/5zvkOQv/nx8rMXmWUMmpohj7c/oG7p3S3Buxsqsu8k/hZG7wa+q/uUrrObmS2xcz5iWP3syPuF2NUsTOz32GPKHDhWHPpoCxQhUUpijBRqanb93Oi9l7kVi/uPieurjt/W12rPed59xd7fRHE8Ygq+bn7+SwZmxP9r2DCN/HVta9SOTtWCixYvR5xQoQWlzr40Jq3jakhCXu8x5hlvo8cC9kL06c8DU8dYqZeJJ6yWnMuMYscR4eLG5x8ZphRpwLOonYh04ts6zHIkqj0nDsMPurAR5aqxdtGcuQTphYS1FA7+HDty0yXFWYZp72NzYulwPzbXlK14b+eAccups5SyNRpIIIoMkObEKL1R5HgCVizY18Iax8/yGwzR9VCP18V288cu4ABFUeIBIGk4OI1yLb3MMdM4VAkykztBSRiFfbDVjrZCkxsYNSSkW3khVjDYTs1eZxhnOLX1L1GC5l58HsZbl8sUSlfP1XOV9XsoJo86bgIS/zSRyA4DTFHR7wg0MJypz13m0AM5ZDVHtiLQCayxq2eA3Zm5geqFOLGWNcBp7DUbecXfzEnB94F9AvWwtSX5tTQxwBARpQViRR8glnWMiYEfHEclXN9yDK8UE8icuxpN2urNQYGUa4ispMZ6qi0ZVcSAg3wFdHtEP0IInwZ5oQKJ5tqxaLXjwjxMDk3SKRAI7X0JSe3cB3znhqqFcaolo9DZmpoYbGd6uoy5uF/y/MXEMYCRphHpj5H/IFGBrZY8IYBuOrk05cQHEImBMbYDABUV2sMiVIYe6Qo9uPNVnrmW0h3E5H8jdb8xHEEFCCOK+NC1c9ylZEdgJRzdGXSUxXbDbWyBO8CKa982k5Pn8Siby3xSjGqKdNceeVWQOyBrKW4YPgA0L8EoHgCVNuqPIilQAzOBT5a9zrxhTZBrclUl8tIsOqQkRz+phSYm3KgsI8LhJ8kQk2YasWE6xQKdJzmM4OAzOKj9e8ghhzAxiHTUIkvsAnO+xAv+rAgVkGQC5DFWjf/pYBD0V1g775q2F/RxGyjANkMAQM+quRmXZ9uJj21jR+5zAFbM3xlbB8iXDXs/7rSwhhPAVUvWtPD9cglP1i90+MLbSa+4KN2Ovm9PtqxB2rLAtSIk0IbAEQ/1NZBZFl50LoZfaxrH82l+5ipytBALfZyaojpSp4q2T+hHtPm7d4PgS0ObG8at7rfAPqvwqVpbeBKMycFtnCRur2cP9gMNZuDBjnqLM/F0V36IxkTA4epqd2htlVk7gPtT01jyc4uXDQSf7fI7T9V6oPXSnWp3ryA1c5WXFoPtBG5/XRprfVfWWmoaz9ibWjFVun5Hbv2pfPBNlL44wQGjWuyUhNfgajPQmModm6xzZwbf86YHCgDXVwJSr5xhtdvdR8BA91Ixj+DOjsFulPURjta9MTNNx21OArvMnTXESPRyz2o+gPAbRBQhqr5OIC+THUlBXceMcZ7I/cov1i/cZ1Qs/4taPUboCsnoWPWAh0dUF/nvQ1bTxijN7YDkhYzGT8uNmxxHiQofbTTA/r1PNRbFdzXATJhgWSMMF1tg+mTvcfYTI6sszJ3uih1KLVMFT4FSOfehzGXlc4zWOu/tBvGXIbapmIXs31A4X4t3h7810iC0TDLhyJfg8h+6s4xc2rwc2iGMw8jMN+23UzpPnsJxuB7yCvVwZQ4D7owDppXvNvuHwEGAPkITPojTuUzAAAAAElFTkSuQmCC" transform="matrix(0.7857 0 0 0.7857 0.8557 11.75)"></image></svg>';

            shareButtons = '<ul class="rwd-share-buttons">';
            shareButtons += $object.settings.share.facebookButton ? '<li><a title="Facebook" id="rwd-share-facebook" class="view7" target="_blank">' + $fb + '</a></li>' : '';
            shareButtons += $object.settings.share.googleplusButton ? '<li><a title="Google Plus" id="rwd-share-googleplus" class="view7" target="_blank">' + $gp + '</a></li>' : '';
            shareButtons += $object.settings.share.twitterButton ? '<li><a title="Twitter" id="rwd-share-twitter" class="view7" target="_blank">' + $twitter + '</a></li>' : '';
            shareButtons += $object.settings.share.vkButton ? '<li><a title="VK" id="rwd-share-vk" class="view7" target="_blank">' + $vk + '</a></li>' : '';
            shareButtons += $object.settings.share.pinterestButton ? '<li><a title="Pinterest" id="rwd-share-pinterest" class="view7" target="_blank">' + $pinterest + '</a></li>' : '';
            shareButtons += $object.settings.share.linkedinButton ? '<li><a title="Linkedin" id="rwd-share-linkedin" class="view7" target="_blank">' + $linkedin + '</a></li>' : '';
            shareButtons += $object.settings.share.tumblrButton ? '<li><a title="Tumblr" id="rwd-share-tumblr" class="view7" target="_blank">' + $tumblr + '</a></li>' : '';
            shareButtons += $object.settings.share.redditButton ? '<li><a title="Reddit" id="rwd-share-reddit" class="view7" target="_blank">' + $reddit + '</a></li>' : '';
            shareButtons += $object.settings.share.diggButton ? '<li><a title="Digg" id="rwd-share-digg" class="view7" target="_blank">' + $digg + '</a></li>' : '';
            shareButtons += $object.settings.share.bufferButton ? '<li><a title="Buffer" id="rwd-share-buffer" class="view7" target="_blank">' + $buffer + '</a></li>' : '';
            shareButtons += $object.settings.share.yummlyButton ? '<li><a title="Yummly" id="rwd-share-yummly" class="view7" target="_blank">' + $yummly + '</a></li>' : '';
            shareButtons += '</ul>';
        }

        switch(this.settings.lightboxView){
            case 'view5':
            case 'view6':
                $('.contInner').append(shareButtons);
                break;
            case 'view7':
                var $view7_share = '', $share = '', socialIcons_ = '', $m_share_event = '';

                $m_share_event = "if(!this.classList.contains('share_open')){this.classList.add('share_open');document.querySelector('.rwd-share-buttons').style.visibility = 'visible';document.querySelector('.rwd-share-buttons').style.opacity = '1';document.querySelector('.rwd-share-buttons').style.display = 'block';if(document.querySelector('.rwd-current .view7_inner').classList.contains('is_open')){document.querySelector('.rwd-current .view7_inner').classList.remove('is_open');document.querySelector('.rwd-current .view7_inner').style.height = 0;document.querySelector('.rwd-close-bar').style.zIndex = 999999999;document.querySelector('.rwd-next').style.zIndex = 999999999;document.querySelector('.rwd-prev').style.zIndex = 999999999;}} else {this.classList.remove('share_open');document.querySelector('.rwd-share-buttons').style.visibility = 'hidden';document.querySelector('.rwd-share-buttons').style.opacity = '0';}";

                $share ='<svg onclick="' + $m_share_event + '" class="rwd-share" version="1.1" id="Layer_1"' +
                    'xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"' +
                    'viewBox="0 0 30 34.5" style="enable-background:new 0 0 30 34.5;" xml:space="preserve">' +
                    '<style type="text/css">.st0 {fill-rule: evenodd;clip-rule: evenodd;fill: #999;}</style><g id="Shape_2">' +
                    '<g id="XMLID_84_"> <path id="XMLID_85_" class="st0"' +
                    'd="M22.1,20.3c-1.3,0-2.5,0.6-3.2,1.6l-7-3.6C12,18,12,17.6,12,17.2c0-0.4-0.1-0.8-0.2-1.2l7-3.6c0.8,1,1.9,1.7,3.3,1.7c2.3,0,4.1-1.9,4.1-4.2c0-2.3-1.8-4.2-4.1-4.2C19.8,5.9,18,7.7,18,10c0,0.4,0.1,0.7,0.2,1.1l-7,3.6c-0.8-1-1.9-1.6-3.2-1.6c-2.3,0-4.1,1.9-4.1,4.2c0,2.3,1.8,4.2,4.1,4.2c1.4,0,2.5-0.7,3.3-1.7l7,3.6c-0.1,0.4-0.2,0.8-0.2,1.2c0,2.3,1.8,4.2,4.1,4.2c2.3,0,4.1-1.9,4.1-4.2C26.2,22.2,24.4,20.3,22.1,20.3z"></path>' +
                    '</g> </g></svg>';

                socialIcons_ = '<div class="share_open_close rwd-icon">' + $share + '</div>';

                var $share_event = '';

                if($(window).width() > 768){
                    $share_event = 'onmouseover="document.querySelector(\'.share_open_close\').style.background = \'rgba(0,0,0,.85)\'; document.querySelector(\'.rwd-share-buttons\').style.visibility = \'visible\'; document.querySelector(\'.rwd-share-buttons\').style.opacity = \'1\'; document.querySelector(\'.rwd-share-buttons\').style.transition = \'visibility 0s, opacity 0.5s linear\'" '+
                        'onmouseout="document.querySelector(\'.share_open_close\').style.background = \'\'; document.querySelector(\'.rwd-share-buttons\').style.visibility = \'hidden\'; document.querySelector(\'.rwd-share-buttons\').style.opacity = \'0\'; document.querySelector(\'.rwd-share-buttons\').style.transition = \'visibility 0.5s, opacity 0.5s linear\'"';
                }

                $view7_share = '<div class="view7_share"' + $share_event + '>' + socialIcons_ + shareButtons + '</div>';

                $('.rwd-container').append($view7_share);
                break;
            default:
                $('.' + this.settings.classPrefix + 'socialIcons').append(shareButtons);
                break;
        }

        setTimeout(function () {
            $('#rwd-share-facebook').attr('href', 'https://www.facebook.com/sharer/sharer.php?u=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-twitter').attr('href', 'https://twitter.com/intent/tweet?text=&url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-googleplus').attr('href', 'https://plus.google.com/share?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-pinterest').attr('href', 'http://www.pinterest.com/pin/create/button/?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-linkedin').attr('href', 'http://www.linkedin.com/shareArticle?mini=true&amp;url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-tumblr').attr('href', 'http://www.tumblr.com/share/link?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-reddit').attr('href', 'http://reddit.com/submit?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-buffer').attr('href', 'https://bufferapp.com/add?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-digg').attr('href', 'http://www.digg.com/submit?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-vk').attr('href', 'http://vkontakte.ru/share.php?url=' + (encodeURIComponent(window.location.href)));
            $('#rwd-share-yummly').attr('href', 'http://www.yummly.com/urb/verify?url=' + (encodeURIComponent(window.location.href)));
        }, 200);
    };

    Lightbox.prototype.changeHash = function (index) {
        var $object = this;

        (($object.settings.socialSharing) && (window.location.hash = '/lightbox&slide=' + (index + 1)));
    };

    Lightbox.prototype.loadContent = function (index, rec, delay) {

        var $object, src, isVideo;

        $object = this;

        function isImg() {
            src = $object.$items.eq(index).attr('href');
            return src.match(/\.(jpg|png|gif)\b/);
        }

        if ($object.settings.watermark) {
            if (isImg()) {
                src = $object.$items.eq(index).find('img').attr('data-src');
            }
        } else {
            src = $object.$items.eq(index).attr('href');
        }

        var $view7_inner = '';

        if(this.settings.lightboxView === 'view7'){
            if(this.settings.showTitle){
                var $t = '<div class="rwd-view7-title"></div>';
            }
            if(this.settings.showDesc){
                var $d = '<div class="rwd-view7-desc"></div>';
            }

            $view7_inner = '<div class="view7_inner is_close">' + $t + $d + '</div>';
        }

        isVideo = $object.isVideo(src, index);
        if (!$object.$item.eq(index).hasClass($object.settings.classPrefix + 'loaded')) {
            if (isVideo) {
                $object.$item.eq(index).addClass('isVideo');
                $object.$item.eq(index).prepend('<div class="' + this.settings.classPrefix + 'video-cont "><div class="' + this.settings.classPrefix + 'video"></div></div>' + $view7_inner);
                $object.$element.trigger('hasVideo.rwd-container', [index, src]);
            } else {
                $object.$item.eq(index).addClass('isImg');
                $object.$item.eq(index).prepend('<div class="' + this.settings.classPrefix + 'img-wrap">' +
                    '<img class="' + this.settings.classPrefix + 'object ' + $object.settings.classPrefix + 'image watermark lightbox_zoom" src="' + src + '" data-zoom-image="' + src + '" />' +
                    $view7_inner +
                    '</div>');
            }

            $object.$element.trigger('onAferAppendSlide.rwd-container', [index]);

            $object.$item.eq(index).addClass($object.settings.classPrefix + 'loaded');
        }

        $object.$item.eq(index).find('.' + $object.settings.classPrefix + 'object').on('load.rwd-container error.rwd-container', function () {

            var speed = 0;
            if (delay) {
                speed = delay;
            }

            setTimeout(function () {
                $object.$item.eq(index).addClass($object.settings.classPrefix + 'complete');
            }, speed);

        });

        if (rec === true) {

            if (!$object.$item.eq(index).hasClass($object.settings.classPrefix + 'complete')) {
                $object.$item.eq(index).find('.' + $object.settings.classPrefix + 'object').on('load.rwd-container error.rwd-container', function () {
                    $object.preload(index);
                });
            } else {
                $object.preload(index);
            }
        }

        if(this.settings.lightboxView !== 'view7' && hugeit_resp_lightbox_obj.hugeit_lightbox_imageframe !== 'frame_0'){
            $('.rwd-image').css({
                borderColor: '#f4be52',
                borderStyle: 'inset',
                borderWidth: '60px'
            });
            if(hugeit_resp_lightbox_obj.hugeit_lightbox_imageframe !== 'frame_8'){
                $('.rwd-image').css({
                    borderImage: 'url("' + hugeit_resp_lightbox_plugins_url + hugeit_resp_lightbox_obj.hugeit_lightbox_imageframe + '.png") 90 95 95 90 stretch stretch'
                });
            } else {
                $('.rwd-image').css({
                    borderImage: 'url("' + hugeit_resp_lightbox_plugins_url + hugeit_resp_lightbox_obj.hugeit_lightbox_imageframe + '.png") 90 123 85 129 stretch stretch'
                });
            }
        }

    };

    Lightbox.prototype.slide = function (index, fromSlide, fromThumb) {
        var $object, prevIndex;

        $object = this;
        prevIndex = this.$cont.find('.' + $object.settings.classPrefix + 'current').index();

        if(this.settings.lightboxView === 'view7'){
            this.innerOpenClose();

            setTimeout(function(){
                var nw, nh;

                if($('.rwd-current').hasClass('isImg')){
                    nw = document.querySelector('.rwd-current img').naturalWidth;
                    nh = document.querySelector('.rwd-current img').naturalHeight;
                } else if($('.rwd-current').hasClass('isVideo')){
                    nw = $('.rwd-current iframe').width();
                    nh = $('.rwd-current iframe').height();
                }

                $('.img_size').html(nw + 'px &times; ' + nh + 'px');
            }, 200);
        }

        var length = this.$item.length,
            time = 0,
            next = false,
            prev = false;

        if (this.settings.download) {
            var src;
            if (!this.settings.watermark) {
                src = $object.$items.eq(index).attr('data-download-url') !== 'false' && ($object.$items.eq(index).attr('data-download-url') || $object.$items.eq(index).attr('href'));
            }
            else {
                src = $object.$items.eq(index).find('img').attr('data-src');
            }
            if (src) {
                $('#' + $object.settings.classPrefix + 'download').attr('href', src);
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-download');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-actual-size');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-fullwidth');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-zoom-in');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-zoom-out');
            } else {
                $object.$cont.addClass($object.settings.classPrefix + 'hide-download');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-actual-size');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-fullwidth');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-zoom-in');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-zoom-out');
            }
        } else {
            if(this.$cont.find('.' + $object.settings.classPrefix + 'item').eq(index).find('iframe').length){
                $object.$cont.addClass($object.settings.classPrefix + 'hide-download');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-actual-size');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-fullwidth');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-zoom-in');
                $object.$cont.addClass($object.settings.classPrefix + 'hide-zoom-out');
            } else {
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-download');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-actual-size');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-fullwidth');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-zoom-in');
                $object.$cont.removeClass($object.settings.classPrefix + 'hide-zoom-out');
            }
        }

        this.$element.trigger('onBeforeSlide.rwd-container', [prevIndex, index, fromSlide, fromThumb]);

        setTimeout(function () {
            $object.setTitle(index);
        }, time);

        if ($object.settings.lightboxView === 'view5' || $object.settings.lightboxView === 'view6' || $object.settings.lightboxView === 'view7') {
            setTimeout(function () {
                $object.setDescription(index);
            }, time);
        }

        this.arrowDisable(index);


        $object.$cont.addClass($object.settings.classPrefix + 'no-trans');

        this.$item.removeClass($object.settings.classPrefix + 'prev-slide ' + $object.settings.classPrefix + 'next-slide');
        if (!fromSlide) {

            if (index < prevIndex) {
                prev = true;
                if ((index === 0) && (prevIndex === length - 1) && !fromThumb) {
                    prev = false;
                    next = true;
                }
            } else if (index > prevIndex) {
                next = true;
                if ((index === length - 1) && (prevIndex === 0) && !fromThumb) {
                    prev = true;
                    next = false;
                }
            }

            if (prev) {

                this.$item.eq(index).addClass($object.settings.classPrefix + 'prev-slide');
                this.$item.eq(prevIndex).addClass($object.settings.classPrefix + 'next-slide');
            } else if (next) {

                this.$item.eq(index).addClass($object.settings.classPrefix + 'next-slide');
                this.$item.eq(prevIndex).addClass($object.settings.classPrefix + 'prev-slide');
            }

            setTimeout(function () {
                $object.$item.removeClass($object.settings.classPrefix + 'current');

                $object.$item.eq(index).addClass($object.settings.classPrefix + 'current');

                $object.$cont.removeClass($object.settings.classPrefix + 'no-trans');
            }, 50);
        } else {

            var slidePrev = index - 1;
            var slideNext = index + 1;

            if ((index === 0) && (prevIndex === length - 1)) {

                slideNext = 0;
                slidePrev = length - 1;
            } else if ((index === length - 1) && (prevIndex === 0)) {

                slideNext = 0;
                slidePrev = length - 1;
            }

            this.$item.removeClass($object.settings.classPrefix + 'prev-slide ' + $object.settings.classPrefix + 'current ' + $object.settings.classPrefix + 'next-slide');
            $object.$item.eq(slidePrev).addClass($object.settings.classPrefix + 'prev-slide');
            $object.$item.eq(slideNext).addClass($object.settings.classPrefix + 'next-slide');
            $object.$item.eq(index).addClass($object.settings.classPrefix + 'current');
        }

        $object.loadContent(index, true, $object.settings.overlayDuration);

        $object.$element.trigger('onAfterSlide.rwd-container', [prevIndex, index, fromSlide, fromThumb]);

        if (this.settings.showCounter) {
            $('#' + $object.settings.classPrefix + 'counter-current').text(index + 1);
        }

        if (this.settings.socialSharing) {
            $object.changeHash(index);
        }

        var $top, $left, $wWidth, $wHeight, $imgWidth, $imgHeight, $wmWidth, $wmHeight, $pos, $item;
        $item = $('.rwd-item.rwd-current');
        $pos = +hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_margin;
        $wWidth = +hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_containerWidth;
        $wHeight = +hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_textFontSize;
        $imgWidth = $object.$item.eq(index).find('img').width();
        $imgHeight = $object.$item.eq(index).find('img').height();
        $wmWidth = $item.width();
        $wmHeight = $item.height();

        switch ('pos' + hugeit_resp_lightbox_obj.hugeit_lightbox_watermark_position_new) {
            case 'pos1':
                $top = ($wmHeight - $imgHeight) / 2 + $pos;
                $left = ($wmWidth - $imgWidth) / 2 + $pos;
                break;
            case 'pos2':
                $top = ($wmHeight - $imgHeight) / 2 + $pos;
                $left = ($wmWidth - $wWidth) / 2;
                break;
            case 'pos3':
                $top = ($wmHeight - $imgHeight) / 2 + $pos;
                $left = ($wmWidth + $imgWidth) / 2 - $wWidth - $pos;
                break;
            case 'pos4':
                $top = ($wmHeight - $wHeight) / 2;
                $left = ($wmWidth - $imgWidth) / 2 + $pos;
                break;
            case 'pos5':
                $top = ($wmHeight - $wHeight) / 2;
                $left = ($wmWidth - $wWidth) / 2;
                break;
            case 'pos6':
                $top = ($wmHeight - $wHeight) / 2;
                $left = ($wmWidth + $imgWidth) / 2 - $wWidth - $pos;
                break;
            case 'pos7':
                $top = ($wmHeight + $imgHeight) / 2 - $wHeight - $pos;
                $left = ($wmWidth - $imgWidth) / 2 + $pos;
                break;
            case 'pos8':
                $top = ($wmHeight + $imgHeight) / 2 - $wHeight - $pos;
                $left = ($wmWidth - $wWidth) / 2;
                break;
            case 'pos9':
                $top = ($wmHeight + $imgHeight) / 2 - $wHeight - $pos;
                $left = ($wmWidth + $imgWidth) / 2 - $wWidth - $pos;
                break;
            default:
                $top = ($wmHeight - $wHeight) / 2;
                $left = ($wmWidth - $wWidth) / 2;
        }

        $('.w_url').css({
            position: 'absolute',
            width: $wWidth + 'px',
            height: $wHeight + 'px',
            top: $top + 'px',
            left: $left + 'px'
        });

        $object.calculateDimensions(index);

        $('.rwd-container .rwd-thumb-item img').css({
            opacity: 1 - 10 / 100
        });

        $('.rwd-container .rwd-thumb-item.active img').css({
            opacity: 1
        });

        if(this.settings.lightboxView === 'view7'){
            this.$element.on('onBeforeSlide.rwd-container', function() {
                $('.view7_share, .rwd-close-bar, .rwd-toolbar').css({
                    visibility: 'hidden',
                    opacity: '0',
                    transition: 'visibility 0s, opacity 0.5s linear'
                });
            });

            this.$element.on('onAfterSlide.rwd-container', function() {
                setTimeout(function(){
                    $('.view7_share, .rwd-close-bar, .rwd-toolbar').css({
                        visibility: 'visible',
                        opacity: '1'
                    });
                }, 450);
            });
        }

        $(window).on('resize.rwd-container orientationchange.rwd-container', function () {
            var $t = ($object.settings.lightboxView !== 'view7') ? 100 : 0;
            setTimeout(function () {
                $object.calculateDimensions(index);
            }, $t);
        });
    };

    Lightbox.prototype.goToNextSlide = function (fromSlide) {
        var $object = this,
            $cont = $('.rwd-cont'),
            $imageObject, k;
        if (($object.index + 1) < $object.$item.length) {
            $object.index++;
            $object.slide($object.index, fromSlide, false);
        } else {
            if ($object.settings.loop === 'true') {
                $object.index = 0;
                $object.slide($object.index, fromSlide, false);
            }
        }

        if ($object.settings.fullwidth && $cont.hasClass('rwd-fullwidth-on')) {
            $imageObject = $cont.find('.rwd-image').eq($object.index);

            k = $imageObject.width() / $imageObject.height();
            if ($imageObject.width() > $imageObject.height() && k > 2) {
                $imageObject.css({
                    'min-width': '100%'
                });
            } else {
                $imageObject.css({
                    'min-height': '100%'
                });
            }
        }
    };

    Lightbox.prototype.goToPrevSlide = function (fromSlide) {
        var $object = this,
            $cont = $('.rwd-cont'),
            $imageObject, k;

        if ($object.index > 0) {
            $object.index--;
            $object.slide($object.index, fromSlide, false);
        } else {
            if ($object.settings.loop === 'true') {
                $object.index = $object.$items.length - 1;
                $object.slide($object.index, fromSlide, false);
            }
        }

        if ($object.settings.fullwidth && $cont.hasClass('rwd-fullwidth-on')) {
            $imageObject = $cont.find('.rwd-image').eq($object.index);

            k = $imageObject.width() / $imageObject.height();
            if ($imageObject.width() > $imageObject.height() && k > 2) {
                $imageObject.css({
                    'min-width': '100%'
                });
            } else {
                $imageObject.css({
                    'min-height': '100%'
                });
            }
        }
    };

    Lightbox.prototype.slideShow = function () {
        var $object = this, $toolbar, $play_bg, $pause_bg;

        $play_bg = '<svg class="play_bg" width="20px" height="20px" fill="#999" viewBox="-192 193.9 314.1 314.1">' +
            '<g><g id="_x33_56._Play"><g><path d="M101,272.5C57.6,197.4-38.4,171.6-113.5,215c-75.1,43.4-100.8,139.4-57.5,214.5c43.4,75.1,139.4,100.8,214.5,57.5C118.6,443.6,144.4,347.6,101,272.5z M27.8,459.7c-60.1,34.7-136.9,14.1-171.6-46c-34.7-60.1-14.1-136.9,46-171.6c60.1-34.7,136.9-14.1,171.6,46C108.5,348.2,87.9,425,27.8,459.7z M21.6,344.6l-82.2-47.9c-7.5-4.4-13.5-0.9-13.5,7.8l0.4,95.2c0,8.7,6.2,12.2,13.7,7.9l81.6-47.1C29,356,29,349,21.6,344.6z"/></g></g></g>' +
            '</svg>';
        $pause_bg = '<svg class="pause_bg" width="20px" height="20px" fill="#999" viewBox="-94 96 510 510" >' +
            '<g><g id="pause-circle-outline"><path d="M84.5,453h51V249h-51V453z M161,96C20.8,96-94,210.8-94,351S20.8,606,161,606s255-114.8,255-255S301.3,96,161,96zM161,555C48.8,555-43,463.2-43,351s91.8-204,204-204s204,91.8,204,204S273.2,555,161,555z M186.5,453h51V249h-51V453z"/></g></g>' +
            '</svg>';

        $toolbar = $('.' + $object.settings.classPrefix + 'toolbar');
        switch (this.settings.lightboxView) {
            case 'view1':
            default:
                $toolbar.append('<span class="' + $object.settings.classPrefix + 'autoplay-button ' + $object.settings.classPrefix + 'icon">' + $play_bg + $pause_bg + '</span>');
                break;
            case 'view2':
                $('.' + $object.settings.classPrefix + 'bar').append('<span class="' + $object.settings.classPrefix + 'autoplay-button ' + $object.settings.classPrefix + 'icon">' + $play_bg + $pause_bg + '</span>');
                break;
            case 'view3':
                $toolbar.append('<span class="' + $object.settings.classPrefix + 'autoplay-button ' + $object.settings.classPrefix + 'icon">' + $play_bg + $pause_bg + '</span>');
                $('.rwd-toolbar .rwd-icon').addClass('rwd-icon0');
                break;
            case 'view4':
                $('<span class="' + $object.settings.classPrefix + 'autoplay-button ' + $object.settings.classPrefix + 'icon">' + $play_bg + $pause_bg + '</span>').insertBefore($('.rwd-title'));
                $('.rwd-toolbar .rwd-icon').addClass('rwd-icon0');
                break;
            case 'view7':
                $('.tool_bar').append('<span class="' + $object.settings.classPrefix + 'autoplay-button ' + $object.settings.classPrefix + 'icon">' + $play_bg + $pause_bg + '</span>');
                break;
        }

        if ($object.settings.slideshowAuto) {
            $object.slideshowAuto();
        }

        $object.$cont.find('.' + $object.settings.classPrefix + 'autoplay-button').on('click.rwd-container', function () {
            !$($object.$cont).hasClass($object.settings.classPrefix + 'show-autoplay') ? $object.startSlide() : $object.stopSlide();
        });

    };

    Lightbox.prototype.slideshowAuto = function () {
        var $object = this;

        $object.$cont.addClass('' + $object.settings.classPrefix + 'show-autoplay');
        $object.startSlide();
    };

    Lightbox.prototype.startSlide = function () {
        var $object = this;
        $object.$cont.addClass('' + $object.settings.classPrefix + 'show-autoplay');
        $('.rwd-autoplay-button > .pause_bg').css({'display': 'inline-block'});
        $('.rwd-autoplay-button > .play_bg').css({'display': 'none'});
        $object.interval = setInterval(function () {
            $object.goToNextSlide();
        }, $object.settings.slideshowSpeed);
    };

    Lightbox.prototype.stopSlide = function () {
        clearInterval(this.interval);
        this.$cont.removeClass(this.settings.classPrefix + 'show-autoplay');
        $('.rwd-thumb').removeClass('thumb_move');
        $('.rwd-autoplay-button > .pause_bg').css({'display': 'none'});
        $('.rwd-autoplay-button > .play_bg').css({'display': 'inline-block'});
    };

    Lightbox.prototype.addKeyEvents = function () {
        var $object = this;
        if (this.$items.length > 1) {
            $(window).on('keyup.rwd-container', function (e) {
                if ($object.$items.length > 1) {
                    if (e.keyCode === 37) {
                        e.preventDefault();
                        $object.goToPrevSlide();
                    }

                    if (e.keyCode === 39) {
                        e.preventDefault();
                        $object.goToNextSlide();
                    }
                }
            });
        }

        $(window).on('keydown.rwd-container', function (e) {
            if ($object.settings.escKey === true && e.keyCode === 27) {
                e.preventDefault();
                if (!$object.$cont.hasClass($object.settings.classPrefix + 'thumb-open')) {
                    $object.destroy();
                } else {
                    $object.$cont.removeClass($object.settings.classPrefix + 'thumb-open');
                }
            }
        });
    };

    Lightbox.prototype.arrow = function () {
        var $object = this;
        this.$cont.find('.' + $object.settings.classPrefix + 'prev').on('click.rwd-container', function () {
            $object.goToPrevSlide();
        });

        this.$cont.find('.' + $object.settings.classPrefix + 'next').on('click.rwd-container', function () {
            $object.goToNextSlide();
        });
    };

    Lightbox.prototype.arrowDisable = function (index) {

        if (!this.settings.loop === 'true' && this.settings.hideControlOnEnd) {
            if ((index + 1) < this.$item.length) {
                this.$cont.find('.' + this.settings.classPrefix + 'next').removeAttr('disabled').removeClass('disabled');
            } else {
                this.$cont.find('.' + this.settings.classPrefix + 'next').attr('disabled', 'disabled').addClass('disabled');
            }

            if (index > 0) {
                this.$cont.find('.' + this.settings.classPrefix + 'prev').removeAttr('disabled').removeClass('disabled');
            } else {
                this.$cont.find('.' + this.settings.classPrefix + 'prev').attr('disabled', 'disabled').addClass('disabled');
            }
        }
    };

    Lightbox.prototype.mousewheel = function () {
        var $object = this, delta;

        $object.$cont.on('mousewheel', function (e) {
            e = e || window.event;
            delta = e.deltaY || e.detail || e.wheelDelta;

            (delta > 0) ? $object.goToNextSlide() : $object.goToPrevSlide();
            e.preventDefault ? e.preventDefault() : (e.returnValue = false);
        });

    };

    Lightbox.prototype.closeGallery = function () {

        var $object = this, mousedown = false;

        this.$cont.find('.' + $object.settings.classPrefix + 'close').on('click.rwd-container', function () {
            $object.destroy();
        });

        if ($object.settings.overlayClose === 'true') {

            $object.$cont.on('mousedown.rwd-container', function (e) {

                mousedown = ($(e.target).is('.' + $object.settings.classPrefix + 'cont') || $(e.target).is('.' + $object.settings.classPrefix + 'item ') || $(e.target).is('.' + $object.settings.classPrefix + 'img-wrap'));

            });

            $object.$cont.on('mouseup.rwd-container', function (e) {

                if ($(e.target).is('.contInner') || $(e.target).is('.' + $object.settings.classPrefix + 'cont') || $(e.target).is('.' + $object.settings.classPrefix + 'item ') || $(e.target).is('.' + $object.settings.classPrefix + 'img-wrap') && mousedown) {
                    if (!$object.$cont.hasClass($object.settings.classPrefix + 'dragEvent')) {
                        $object.destroy();
                    }
                }

            });

        }
    };

    Lightbox.prototype.destroy = function (d) {

        var $object = this, $time;

        $('.rwd-container').removeClass(this.settings.openCloseType[0]).addClass(this.settings.openCloseType[1]);

        switch(this.settings.openCloseType[1]){
            case 'close_1':
            case 'close_1_r':
                $time = 1000;
                break;
            case 'close_2':
            case 'close_2_r':
                $time = 300;
                break;
            case 'close_3':
            case 'close_4':
            case 'close_3_r':
            case 'close_4_r':
                $time = 340;
                break;
            case 'close_5':
            case 'close_5_r':
                $time = 250;
                break;
        }

        $('html, body').on('mousewheel', function(){
            return false;
        });

        setTimeout(function(){
            clearInterval($object.interval);

            $object.$body.removeClass($object.settings.classPrefix + 'on');

            $(window).scrollTop($object.prevScrollTop);

            if (d) {
                $.removeData($object.el, 'lightbox');
            }

            ($object.settings.socialSharing && (window.location.hash = ''));

            $object.$element.off('.rwd-container');

            $(window).off('.rwd-container');

            if ($object.$cont) {
                $object.$cont.removeClass($object.settings.classPrefix + 'visible');
            }

            $object.objects.overlay.removeClass('in');

            setTimeout(function () {
                if ($object.$cont) {
                    $object.$cont.remove();
                }

                $object.objects.overlay.remove();

            }, $object.settings.overlayDuration + 50);

            window.scrollTo(0, $object.$_y_);

            $.fn.lightbox.lightboxModul['modul'].prototype.destroy();

            $('html, body').off('mousewheel');
        }, $time);
    };

    $.fn.lightbox = function (options) {
        return this.each(function () {
            if (!$.data(this, 'lightbox')) {
                $.data(this, 'lightbox', new Lightbox(this, options));
            }
        });
    };

    $.fn.lightbox.lightboxModul = {};

    var Modul = function (element) {

        this.dataL = $(element).data('lightbox');
        this.$element = $(element);
        this.dataL.modulSettings = $.extend({}, this.constructor.defaultsModul);

        this.init();

        if (this.dataL.modulSettings.zoom && this.dataL.effectsSupport()) {
            if(hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView !== 'view7' && this.dataL.modulSettings.zoomType === '0'){
                this.initZoom();

                this.zoomabletimeout = false;

                this.pageX = $(window).width() / 2;
                this.pageY = ($(window).height() / 2) + $(window).scrollTop();
            }
        }

        if (hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView !== 'view7' && this.dataL.modulSettings.fullwidth && this.dataL.effectsSupport()) {
            this.initFullWidth();
        }

        this.$el = $(element);
        this.$thumbCont = null;
        this.thumbContWidth = 0;
        this.thumbTotalWidth = (this.dataL.$items.length * (this.dataL.modulSettings.thumbsWidth + this.dataL.modulSettings.thumbMargin));
        this.thumbIndex = this.dataL.index;
        this.left = 0;
        if(hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs === 'true'){
            this.initThumbs();
        }

        if(this.dataL.modulSettings.fullscreen){
            this.initFullscreen();
        }

        return this;
    };

    Modul.defaultsModul = {
        idPrefix: 'rwd-',
        classPrefix: 'rwd-',
        attrPrefix: 'data-',
        videoMaxWidth: '700',
        fullwidth: hugeit_resp_lightbox_obj.hugeit_lightbox_fullwidth_effect === 'true',
        zoom: hugeit_resp_lightbox_obj.hugeit_lightbox_zoom,
        scale: +hugeit_resp_lightbox_obj.hugeit_lightbox_zoomsize / 10,
        thumbnail: hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs,
        thumbsWidth: 100,
        thumbsHeight: 100,
        thumbMargin: 5,
        showByDefault: true,
        toogleThumb: false,
        thumbPosition: '0',
        thumbsOverlayColor: 'black',
        thumbsOverlayOpacity: 10,
        fullscreen: hugeit_resp_lightbox_obj.hugeit_lightbox_fullscreen_effect === 'true'
    };

    Modul.prototype.init = function () {
        var $object = this;

        $object.dataL.$element.on('hasVideo.rwd-container', function (event, index, src) {
            $object.dataL.$item.eq(index).find('.' + $object.dataL.modulSettings.classPrefix + 'video').append($object.loadVideo(src, '' + $object.dataL.modulSettings.classPrefix + 'object', index));
        });

        $object.dataL.$element.on('onAferAppendSlide.rwd-container', function (event, index) {
            $object.dataL.$item.eq(index).find('.' + $object.dataL.settings.classPrefix + 'video-cont').css({
                'max-width': $object.dataL.modulSettings.videoMaxWidth + 'px'
            });
        });

        $object.dataL.$element.on('onBeforeSlide.rwd-container', function (event, prevIndex, index) {

            var $videoSlide = $object.dataL.$item.eq(prevIndex),
                youtubePlayer = $videoSlide.find('.rwd-youtube').get(0),
                vimeoPlayer = $videoSlide.find('.rwd-vimeo').get(0),
                dailymotionPlayer = $videoSlide.find('.rwd-dailymotion').get(0);

            if (youtubePlayer) {
                youtubePlayer.contentWindow.postMessage('{"event":"command","func":"pauseVideo","args":""}', '*');
            } else if (vimeoPlayer) {
                try {
                    $f(vimeoPlayer).api('pause');
                } catch (e) {
                    console.error('Make sure you have included froogaloop2 js');
                }
            } else if (dailymotionPlayer) {
                dailymotionPlayer.contentWindow.postMessage('pause', '*');
            }

            var src;
            src = $object.dataL.$items.eq(index).attr('href');

            var isVideo = $object.dataL.isVideo(src, index) || {};
            if (isVideo.youtube || isVideo.vimeo || isVideo.dailymotion) {
                $object.dataL.$cont.addClass($object.dataL.modulSettings.classPrefix + 'hide-download');
                $object.dataL.$cont.addClass($object.dataL.modulSettings.classPrefix + 'hide-actual-size');
                $object.dataL.$cont.addClass($object.dataL.modulSettings.classPrefix + 'hide-fullwidth');
                $object.dataL.$cont.addClass($object.dataL.modulSettings.classPrefix + 'hide-zoom-in');
                $object.dataL.$cont.addClass($object.dataL.modulSettings.classPrefix + 'hide-zoom-out');
            }

        });

        $object.dataL.$element.on('onAfterSlide.rwd-container', function (event, prevIndex) {
            $object.dataL.$item.eq(prevIndex).removeClass($object.dataL.modulSettings.classPrefix + 'video-playing');
        });
    };

    Modul.prototype.loadVideo = function (src, addClass, index) {
        var video = '',
            autoplay = 0,
            a = '',
            isVideo = this.dataL.isVideo(src, index) || {};

        if (isVideo.youtube) {

            a = '?wmode=opaque&autoplay=' + autoplay + '&enablejsapi=1';

            video = '<iframe class="' + this.dataL.modulSettings.classPrefix + 'video-object ' + this.dataL.modulSettings.classPrefix + 'youtube ' + addClass + '" width="560" height="315" src="//www.youtube.com/embed/' + isVideo.youtube[1] + a + '" frameborder="0" allowfullscreen></iframe>';

        } else if (isVideo.vimeo) {

            a = '?autoplay=' + autoplay + '&api=1';

            video = '<iframe class="' + this.dataL.modulSettings.classPrefix + 'video-object ' + this.dataL.modulSettings.classPrefix + 'vimeo ' + addClass + '" width="560" height="315"  src="//player.vimeo.com/video/' + isVideo.vimeo[1] + a + '" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>';

        } else if (isVideo.dailymotion) {

            a = '?wmode=opaque&autoplay=' + autoplay + '&api=postMessage';

            video = '<iframe class="rwd-video-object rwd-dailymotion ' + addClass + '" width="560" height="315" src="//www.dailymotion.com/embed/video/' + isVideo.dailymotion[2] + a + '" frameborder="0" allowfullscreen></iframe>';

        }

        return video;
    };

    Modul.prototype.initFullscreen = function() {
        var fullScreen = '';
        if (this.dataL.modulSettings.fullscreen) {
            if (!document.fullscreenEnabled && !document.webkitFullscreenEnabled && !document.mozFullScreenEnabled && !document.msFullscreenEnabled) {
                return;
            } else {
                fullScreen = '<span class="rwd-fullscreen rwd-icon">' +
                    '<svg id="rwd-fullscreen-on" width="20px" height="20px" stroke="#999" fill="#999" x="0px" y="0px" viewBox="134 -133 357 357" style="enable-background:new 134 -133 357 357;">' +
                    '<g><g id="fullscreen"><path d="M165,96.5h-31V224h127.5v-31H165V96.5z M134-5.5h31V-82h96.5v-31H134V-5.5z M440,193h-76.5v31H491V96.5h-31V192z M363.5-103v21H460v76.5h31V-113H363.5z"></path>' +
                    '</g></g></svg>' +
                    '<svg id="rwd-fullscreen-off" width="20px" height="20px" stroke="#999" fill="#999" x="0px" y="0px" viewBox="134 -133 357 357" style="enable-background:new 134 -133 357 357;">' +
                    '<g><g id="fullscreen-exit"><path d="M134, 127.5h 96.5V 224h 31V 96.5H 114V 147.5z M210.5 -36.5H 134v 31h 127.5V -133h -31V -36.5z M363.5, 224h 31v -96.5H 491v -31H 363.5V 224z M394.5 -56.5V -133h -31V -5.5H 491v -31H 395.5z"></path>' +
                    '</g></g></svg>' +
                    '</span>';
                switch (hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView) {
                    case 'view1':
                    default:
                        $('.rwd-cont').find('.rwd-toolbar').append(fullScreen);
                        break;
                    case 'view2':
                        $('.rwd-cont').find('.rwd-bar').append(fullScreen);
                        break;
                    case 'view4':
                        $(fullScreen).insertBefore('.rwd-title');
                        break;
                    case 'view7':
                        $('.tool_bar').append(fullScreen);
                        break;
                }

                this.fullScreen();
            }
        }
    };

    Modul.prototype.requestFullscreen = function() {
        var el = document.documentElement;

        if (el.requestFullscreen) {
            el.requestFullscreen();
        } else if (el.msRequestFullscreen) {
            el.msRequestFullscreen();
        } else if (el.mozRequestFullScreen) {
            el.mozRequestFullScreen();
        } else if (el.webkitRequestFullscreen) {
            el.webkitRequestFullscreen();
        }
    };

    Modul.prototype.exitFullscreen = function() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    };

    Modul.prototype.fullScreen = function() {
        var $object = this;

        $(document).on('fullscreenchange.rwd-container webkitfullscreenchange.rwd-container mozfullscreenchange.rwd-container MSFullscreenChange.rwd-container', function() {
            $('.rwd-cont').toggleClass('rwd-fullscreen-on');
        });

        $('.rwd-cont').find('.rwd-fullscreen').on('click.rwd-container', function() {
            if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {
                $object.requestFullscreen();
            } else {
                $object.exitFullscreen();
            }
        });

        $(window).on('keydown', function(e){
           if(e.keyCode === 27){
               console.log(1);
           }
        });

    };

    Modul.prototype.initFullWidth = function () {
        var $object = this,
            $fullWidth, $fullWidthOn;

        $fullWidth = '<svg id="rwd-fullwidth" width="50px" height="25px" stroke="#999" fill="#999" x="0px" y="0px" viewBox="0 0 960 500" style="enable-background:new 0 0 960 560;">' +
            '<g><path d="M769.4,280L651.7,397.6l0.1-90.5L543.1,307l0-54.3l108.6,0.1l0.1-90.5L769.4,280z M416.4,306.9l-108.6-0.1l-0.1,90.5' +
            'L190.2,279.6L307.9,162l-0.1,90.5l108.6,0.1L416.4,306.9z M416.4,306.9"/></g></svg>';

        $fullWidthOn = '<svg  id="rwd-fullwidth_on" width="50px" height="25px" stroke="#999" fill="#999" x="0px" y="0px" viewBox="0 0 960 500" style="enable-background:new 0 0 960 560;">' +
            '<path d="M516,280.3l117.3-118l0.3,90.5l108.6-0.3l0.2,54.3l-108.6,0.3l0.3,90.5L516,280.3z M217.3,252.8l108.6-0.3l-0.2-90.5' +
            'l117.9,117.4l-117.4,118l-0.2-90.5l-108.6,0.3L217.3,252.8z M416.4,306.9"/></svg>';

        if (this.dataL.modulSettings.fullwidth) {
            var fullwidth = '<span class="rwd-fullwidth rwd-icon">' + $fullWidth + $fullWidthOn + '</span>';
            switch (hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView) {
                case 'view1':
                default:
                    this.dataL.$cont.find('.rwd-toolbar').append(fullwidth);
                    break;
                case 'view2':
                    this.dataL.$cont.find('.rwd-bar').append(fullwidth);
                    break;
                case 'view4':
                    $(fullwidth).insertBefore('.rwd-title');
                    break;
            }

        }

        if (this.dataL.modulSettings.fullwidth) {
            $('.rwd-fullwidth').on('click.rwd-container', function () {
                !$('.rwd-cont').hasClass('rwd-fullwidth-on') ? $object.onFullWidth() : $object.offFullWidth();
            });
        }
    };

    Modul.prototype.onFullWidth = function () {

        var $imageObject = this.dataL.$cont.find('.rwd-current .rwd-image');

        $('#rwd-fullwidth').css({'display': 'none'});
        $('#rwd-fullwidth_on').css({'display': 'inline-block'});

        $('.rwd-cont').addClass('rwd-fullwidth-on');

        $('.rwd-container').css({
            width: '100%',
            height: '100%'
        });

        var k = $imageObject.width() / $imageObject.height();
        if ($imageObject.width() > $imageObject.height() && k > 2) {
            $imageObject.css({
                'min-width': '100%'
            });
        } else {
            $imageObject.css({
                'min-height': '100%'
            });
        }
    };

    Modul.prototype.offFullWidth = function () {
        var $imageObject = this.dataL.$cont.find('.rwd-current .rwd-image');

        $('#rwd-fullwidth').css({'display': 'inline-block'});
        $('#rwd-fullwidth_on').css({'display': 'none'});

        $('.rwd-cont').removeClass('rwd-fullwidth-on');
        $('.rwd-container').css({
            width: hugeit_resp_lightbox_obj.hugeit_lightbox_width_new + '%',
            height: hugeit_resp_lightbox_obj.hugeit_lightbox_height_new + '%'
        });
        $imageObject.css({
            'min-width': '',
            'min-height': ''
        });
    };

    Modul.prototype.initZoom = function () {

        var $object = this, zoomIcons,
            $zoomIn, $zoomOut, scale;

        $zoomIn = '<svg id="zoom_in" width="20px" height="20px" stroke="#999" fill="#999" x="0px" y="0px" viewBox="-18 19 53 53" style="enable-background:new -18 19 53 53;">' +
            '<g><path d="M11,39H5v-6c0-0.6-0.4-1-1-1s-1,0.4-1,1v6h-6c-0.6,0-1,0.4-1,1s0.4,1,1,1h6v6c0,0.6,0.4,1,1,1s1-0.4,1-1v-6h6' +
            'c0.6,0,1-0.4,1-1S11.5,39,11,39z"/>' +
            '<path d="M33.7,70.3L18.8,54.9c3.8-3.8,6.1-9,6.1-14.8c0-11.6-9.4-21-21-21s-21,9.4-21,21s9.4,21,21,21c5.1,0,9.7-1.8,13.4-4.8' +
            'l14.9,15.5c0.2,0.2,0.5,0.3,0.7,0.3c0.3,0,0.5-0.1,0.7-0.3C34.1,71.3,34.1,70.7,33.7,70.3z M-15,40c0-10.5,8.5-19,19-19' +
            's19,8.5,19,19S14.5,59,4,59S-15,50.5-15,40z"/></g>' +
            '</svg>';

        $zoomOut = '<svg id="zoom_out" width="20px" height="20px" stroke="#999" fill="#999" x="0px" y="0px" x="0px" y="0px" viewBox="-18 19 53 53" style="enable-background:new -18 19 53 53;">' +
            '<g><path d="M11,39H-3c-0.6,0-1,0.4-1,1s0.4,1,1,1h14c0.6,0,1-0.4,1-1S11.5,39,11,39z"/>' +
            '<path d="M33.7,70.3L18.8,54.9c3.8-3.8,6.1-9,6.1-14.8c0-11.6-9.4-21-21-21s-21,9.4-21,21s9.4,21,21,21c5.1,0,9.7-1.8,13.4-4.8' +
            'l14.9,15.5c0.2,0.2,0.5,0.3,0.7,0.3c0.3,0,0.5-0.1,0.7-0.3C34.1,71.3,34.1,70.7,33.7,70.3z M-15,40c0-10.5,8.5-19,19-19' +
            's19,8.5,19,19S14.5,59,4,59S-15,50.5-15,40z"/></g>' +
            '</svg>';

        zoomIcons = '<span id="rwd-zoom-out" class="rwd-icon">' + $zoomOut + '</span><span id="rwd-zoom-in" class="rwd-icon">' + $zoomIn + '</span>';

        switch (hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView) {
            case 'view1':
            default:
                this.dataL.$cont.find('.rwd-toolbar').append(zoomIcons);
                break;
            case 'view2':
                this.dataL.$cont.find('.rwd-bar').append(zoomIcons);
                break;
            case 'view4':
                $(zoomIcons).insertBefore('.rwd-title');
                break;
        }

        scale = 1;
        function zoom(scaleVal) {
            var $imageObject, _x, _y, offsetX, offsetY, x, y;

            $imageObject = $object.dataL.$cont.find('.rwd-current .rwd-image');

            offsetX = ($(window).width() - $imageObject.width()) / 2;
            offsetY = (($(window).height() - $imageObject.height()) / 2) + $(window).scrollTop();

            _x = $object.pageX - offsetX;
            _y = $object.pageY - offsetY;

            x = _x;
            y = _y;

            $imageObject.css('transform', 'scale3d(' + scaleVal + ', ' + scaleVal + ', 1)').attr('data-scale', scaleVal);

            $imageObject.parent().css({
                transform: 'translate3d(0, ' + -y + 'px, 0)'
            }).attr('data-y', -y);
        }

        function callScale() {
            if (scale > 1) {
                $object.dataL.$cont.addClass('rwd-zoomed');
            } else {
                $object.dataL.$cont.removeClass('rwd-zoomed');
            }

            if (scale < 1) {
                scale = 1;
            }

            zoom(scale);
        }

        $(window).on('resize.rwd-container.zoom scroll.rwd-container.zoom orientationchange.rwd-container.zoom', function () {
            $object.pageX = $(window).width() / 2;
            $object.pageY = ($(window).height() / 2) + $(window).scrollTop();
            zoom(scale);
        });

        $('#rwd-zoom-out').on('click.rwd-container', function () {
            if ($object.dataL.$cont.find('.rwd-current .rwd-image').length) {
                scale -= $object.dataL.modulSettings.scale;
                callScale();
            }
        });

        $('#rwd-zoom-in').on('click.rwd-container', function () {
            if ($object.dataL.$cont.find('.rwd-current .rwd-image').length) {
                scale += $object.dataL.modulSettings.scale;
                callScale();
            }
        });

        if (hugeit_resp_lightbox_obj.hugeit_lightbox_zoomlogo !== '0') {
            $object.dataL.$cont.dblclick(function () {
                if (!$object.dataL.$cont.hasClass('dbl-zoomed')) {
                    $object.dataL.$cont.addClass('dbl-zoomed');
                    if ($object.dataL.$cont.find('.rwd-current .rwd-image').length) {
                        scale += $object.dataL.modulSettings.scale;
                        callScale();
                    }
                } else {
                    $object.dataL.$cont.removeClass('dbl-zoomed');
                    if ($object.dataL.$cont.find('.rwd-current .rwd-image').length) {
                        scale -= $object.dataL.modulSettings.scale;
                        callScale();
                    }
                }
            });
        }

        if (!('ontouchstart' in document.documentElement)) {
            $object.zoomDrag();
        }

        if (('ontouchstart' in document.documentElement)) {
            $object.zoomSwipe();
        }

    };

    Modul.prototype.touchendZoom = function (startCoords, endCoords, abscissa, ordinate) {

        var $object = this, _$el, $imageObject, distanceX, distanceY, maxX, maxY;

        _$el = $object.dataL.$item.eq($object.dataL.index).find('.rwd-img-wrap');
        $imageObject = $object.dataL.$item.eq($object.dataL.index).find('.rwd-object');
        maxX = Math.abs($imageObject.outerWidth() * Math.abs($imageObject.attr('data-scale')) - $object.dataL.$cont.find('.rwd-container').width()) / 2;
        maxY = Math.abs($imageObject.outerHeight() * Math.abs($imageObject.attr('data-scale')) - $object.dataL.$cont.find('.rwd-container').height()) / 2 + $(window).scrollTop();

        if (_$el.attr('data-x')) {
            distanceX = +_$el.attr('data-x') + (endCoords.x - startCoords.x);
        } else {
            distanceX = endCoords.x - startCoords.x;
        }

        distanceY = +_$el.attr('data-y') + (endCoords.y - startCoords.y);

        if ((Math.abs(endCoords.x - startCoords.x) > 15) || (Math.abs(endCoords.y - startCoords.y) > 15)) {

            if (abscissa) {
                if (endCoords.x - startCoords.x < 0) {
                    if(distanceX <= -maxX){
                        distanceX = -maxX;
                    }
                } else {
                    if(distanceX >= maxX) {
                        distanceX = maxX;
                    }
                }

                _$el.attr('data-x', distanceX);
            }

            if (ordinate) {
                if (endCoords.y - startCoords.y < 0) {
                    if(distanceY <= -(maxY + ($object.pageY - ($(window).height() - $imageObject.height()) / 2)) + 2 * $(window).scrollTop()) {
                        distanceY = -(maxY + ($object.pageY - ($(window).height() - $imageObject.height()) / 2)) + 2 * $(window).scrollTop();
                    }
                } else {
                    if(distanceY >= maxY - ($object.pageY - ($(window).height() - $imageObject.height()) / 2)) {
                        distanceY = maxY - ($object.pageY - ($(window).height() - $imageObject.height()) / 2);
                    }
                }

                _$el.attr('data-y', distanceY);
            }

            _$el.css({
                transform: 'translate3d(' + distanceX + 'px, ' + distanceY + 'px, 0)'
            });

        }
    };

    Modul.prototype.zoomDrag = function () {

        var $object = this,
            startCoords = {},
            endCoords = {},
            isDraging = false,
            isMoved = false,
            abscissa = false,
            ordinate = false;

        $object.dataL.$item.on('mousedown.rwd-container.zoom', function (e) {

            var $imageObject = $object.dataL.$item.eq($object.dataL.index).find('.rwd-object');

            ordinate = $imageObject.outerHeight() * $imageObject.attr('data-scale') > $object.dataL.$cont.find('.rwd-container').height();
            abscissa = $imageObject.outerWidth() * $imageObject.attr('data-scale') > $object.dataL.$cont.find('.rwd-container').width();

            if ($object.dataL.$cont.hasClass('rwd-zoomed')) {
                if ($(e.target).hasClass('rwd-object') && (abscissa || ordinate)) {
                    e.preventDefault();
                    startCoords = {
                        x: e.pageX,
                        y: e.pageY
                    };

                    isDraging = true;

                    $object.dataL.$cont.scrollLeft += 1;
                    $object.dataL.$cont.scrollLeft -= 1;

                }
            }
        });

        $(window).on('mousemove.rwd-container.zoom', function (e) {
            if (isDraging) {
                var _$el = $object.dataL.$item.eq($object.dataL.index).find('.rwd-img-wrap');
                var distanceX;
                var distanceY;

                isMoved = true;
                endCoords = {
                    x: e.pageX,
                    y: e.pageY
                };

                if (_$el.attr('data-x')) {
                    distanceX = +_$el.attr('data-x') + (endCoords.x - startCoords.x);
                } else {
                    distanceX = endCoords.x - startCoords.x;
                }

                if (ordinate) {
                    distanceY = +_$el.attr('data-y') + (endCoords.y - startCoords.y);
                }

                _$el.css({
                    transform: 'translate3d(' + distanceX + 'px, ' + distanceY + 'px, 0)'
                });
            }
        });

        $(window).on('mouseup.rwd-container.zoom', function (e) {

            if (isDraging) {
                isDraging = false;

                if (isMoved && ((startCoords.x !== endCoords.x) || (startCoords.y !== endCoords.y))) {
                    endCoords = {
                        x: e.pageX,
                        y: e.pageY
                    };
                    $object.touchendZoom(startCoords, endCoords, abscissa, ordinate);

                }

                isMoved = false;
            }

        });
    };

    Modul.prototype.zoomSwipe = function () {
        var $object = this,
            startCoords = {},
            endCoords = {},
            isMoved = false,
            abscissa = false,                   ordinate = false;

        $object.dataL.$item.on('touchstart.rwd-container', function (e) {

            if ($object.dataL.$cont.hasClass('rwd-zoomed')) {
                var $imageObject = $object.dataL.$item.eq($object.dataL.index).find('.rwd-object');

                ordinate = $imageObject.outerHeight() * $imageObject.attr('data-scale') > $object.dataL.$cont.find('.rwd-container').height();
                abscissa = $imageObject.outerWidth() * $imageObject.attr('data-scale') > $object.dataL.$cont.find('.rwd-container').width();
                if ((abscissa || ordinate)) {
                    e.preventDefault();
                    startCoords = {
                        x: e.originalEvent.targetTouches[0].pageX,
                        y: e.originalEvent.targetTouches[0].pageY
                    };
                }
            }

        });

        $object.dataL.$item.on('touchmove.rwd-container', function (e) {

            if ($object.dataL.$cont.hasClass('rwd-zoomed')) {

                var _$el = $object.dataL.$item.eq($object.dataL.index).find('.rwd-img-wrap');
                var distanceX;
                var distanceY;

                e.preventDefault();
                isMoved = true;

                endCoords = {
                    x: e.originalEvent.targetTouches[0].pageX,
                    y: e.originalEvent.targetTouches[0].pageY
                };

                if (_$el.attr('data-x')) {
                    distanceX = +_$el.attr('data-x') + (endCoords.x - startCoords.x);
                } else {
                    distanceX = endCoords.x - startCoords.x;
                }

                if (ordinate) {
                    distanceY = +_$el.attr('data-y') + (endCoords.y - startCoords.y);
                }

                if ((Math.abs(endCoords.x - startCoords.x) > 15) || (Math.abs(endCoords.y - startCoords.y) > 15)) {
                    _$el.css({
                        transform: 'translate3d(' + distanceX + 'px, ' + distanceY + 'px, 0)'
                    });
                }

            }

        });

        $object.dataL.$item.on('touchend.rwd-container', function () {
            if ($object.dataL.$cont.hasClass('rwd-zoomed')) {
                if (isMoved) {
                    isMoved = false;
                    $object.touchendZoom(startCoords, endCoords, abscissa, ordinate);

                }
            }
        });

    };

    Modul.prototype.initThumbs = function() {
        var $object = this;

        if (this.dataL.modulSettings.thumbnail === 'true' && this.dataL.$items.length > 1) {

            if (this.dataL.modulSettings.showByDefault) {
                setTimeout(function(){
                    $object.dataL.$cont.addClass('rwd-thumb-open');
                }, 100);
            }

            this.buildThumbs();

            this.dataL.effectsSupport() && this.enableThumbDrag();

            this.activatedThumbs = false;

            if ($object.dataL.modulSettings.toogleThumb) {
                $object.$thumbCont.append('<span class="rwd-toggle-thumb rwd-icon"></span>');
                $object.dataL.$cont.find('.rwd-toggle-thumb').on('click.rwd-container', function() {
                    $object.dataL.$cont.toggleClass('rwd-thumb-open');
                });
            }
        }

        $('.rwd-container .rwd-thumb-item').css({
            background: '#' + this.dataL.modulSettings.thumbsOverlayColor
        });
        $('.rwd-container .rwd-thumb-item img').css({
            opacity: 1 - +this.dataL.modulSettings.thumbsOverlayOpacity / 100
        });

        $('.rwd-thumb-cont').css({
            bottom: -$object.dataL.modulSettings.thumbsHeight + 'px'
        });

        if (this.dataL.modulSettings.showByDefault) {
            var $cont_ = $('.cont-inner'),
                $thumb_ = $('.rwd-thumb-cont'),
                $toolbar_ = $('.rwd-toolbar');
            setTimeout(function(){
                switch(hugeit_resp_lightbox_obj.hugeit_lightbox_lightboxView){
                    case 'view1':
                        switch($object.dataL.modulSettings.thumbPosition){
                            case '0':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)',
                                    top: '47px'
                                });
                                $thumb_.css({
                                    bottom: '0',
                                    backgroundColor: 'rgba(0,0,0,.9)'
                                });
                                $('.rwd-bar > *').css({
                                    bottom: $object.dataL.modulSettings.thumbsHeight + 'px'
                                });
                                break;
                            case '1':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)',
                                    top: $object.dataL.modulSettings.thumbsHeight + 47 + 'px'
                                });
                                $thumb_.css({
                                    top: '47px',
                                    backgroundColor: 'rgba(0,0,0,.9)'
                                });
                                break;
                        }
                        break;
                    case 'view2':
                        switch($object.dataL.modulSettings.thumbPosition) {
                            case '0':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)',
                                    top: '45px'
                                });
                                $thumb_.css({
                                    bottom: '45px',
                                    backgroundColor: 'rgba(0,0,0,.9)'
                                });
                                break;
                            case '1':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)',
                                    top: $object.dataL.modulSettings.thumbsHeight + 45 + 'px'
                                });
                                $thumb_.css({
                                    top: '0',
                                    backgroundColor: 'rgba(0,0,0,.9)'
                                });
                                $toolbar_.css({
                                    top: $object.dataL.modulSettings.thumbsHeight + 'px'
                                });
                                break;
                        }
                        break;
                    case 'view3':
                        switch($object.dataL.modulSettings.thumbPosition) {
                            case '0':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)',
                                    top: '47px'
                                });
                                $thumb_.css({
                                    bottom: '0',
                                    backgroundColor: 'white'
                                });
                                $('.rwd-title').css({
                                    bottom: $object.dataL.modulSettings.thumbsHeight + 'px'
                                });
                                break;
                            case '1':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 93) + 'px)',
                                    top: ($object.dataL.modulSettings.thumbsHeight + 48) + 'px'
                                });
                                $thumb_.css({
                                    top: '48px',
                                    backgroundColor: 'white'
                                });
                                break;
                        }
                        break;
                    case 'view4':
                    case 'view7':
                        switch($object.dataL.modulSettings.thumbPosition) {
                            case '0':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 92) + 'px)'
                                });
                                $thumb_.css({
                                    bottom: '0',
                                    backgroundColor: 'none'
                                });
                                $('.rwd-socialIcons').css({
                                    bottom: ($object.dataL.modulSettings.thumbsHeight - 10) + 'px'
                                });
                                $('.barCont').css({
                                    bottom: $object.dataL.modulSettings.thumbsHeight + 'px'
                                });
                                $('#rwd-counter').css({
                                    bottom: ($object.dataL.modulSettings.thumbsHeight + 5) + 'px'
                                });
                                $('.rwd-item').css({
                                    top: '47px'
                                });
                                break;
                            case '1':
                                $cont_.css({
                                    height: 'calc(100% - ' + ($object.dataL.modulSettings.thumbsHeight + 90) + 'px)',
                                    top: $object.dataL.modulSettings.thumbsHeight + 45 + 'px'
                                });
                                $thumb_.css({
                                    top: '45px',
                                    backgroundColor: 'none'
                                });
                                break;
                        }
                        break;
                    case 'view5':
                    case 'view6':
                        switch($object.dataL.modulSettings.thumbPosition) {
                            case '0':
                                $cont_.css({
                                    height: 'calc(100% - ' + $object.dataL.modulSettings.thumbsHeight + 'px)'
                                });
                                $thumb_.css({
                                    bottom: '0'
                                });
                                break;
                            case '1':
                                $cont_.css({
                                    height: 'calc(100% - ' + $object.dataL.modulSettings.thumbsHeight + 'px)',
                                    top: $object.dataL.modulSettings.thumbsHeight + 'px'
                                });
                                $thumb_.css({
                                    top: '0'
                                });
                                break;
                        }
                        break;
                }
            }, 100);
        }
    };

    Modul.prototype.buildThumbs = function() {
        var $object = this,
            thumbList = '',
            $thumb,
            html = '<div class="rwd-thumb-cont"><div class="rwd-thumb group"></div></div>';

        $object.dataL.$cont.addClass('rwd-has-thumb');

        $object.dataL.$cont.find('.rwd-container').append(html);

        $object.$thumbCont = $object.dataL.$cont.find('.rwd-thumb-cont');
        $object.thumbContWidth = $object.$thumbCont.width();

        $object.dataL.$cont.find('.rwd-thumb').css({
            width: $object.thumbTotalWidth + 'px',
            position: 'relative'
        });

        $object.$thumbCont.css('height', $object.dataL.modulSettings.thumbsHeight + 'px');

        function getThumb(src, thumb, index) {
            var isVideo = $object.dataL.isVideo(src, index) || {};
            var thumbImg;
            var vimeoId = '';

            if (isVideo.youtube || isVideo.vimeo || isVideo.dailymotion) {
                if (isVideo.youtube) {
                    thumbImg = '//img.youtube.com/vi/' + isVideo.youtube[1] + '/1.jpg';
                } else if (isVideo.vimeo) {
                    thumbImg = '//i.vimeocdn.com/video/error_100x75.jpg';
                    vimeoId = isVideo.vimeo[1];
                } else if (isVideo.dailymotion) {
                    thumbImg = '//www.dailymotion.com/thumbnail/video/' + isVideo.dailymotion[2];
                }
            } else {
                thumbImg = thumb;
            }

            thumbList += '<div data-vimeo-id="' + vimeoId + '" class="rwd-thumb-item" style="width:' + $object.dataL.modulSettings.thumbsWidth + 'px; margin-right: ' + $object.dataL.modulSettings.thumbMargin + 'px"><img src="' + thumbImg + '" /></div>';
            vimeoId = '';
        }

        $object.dataL.$items.each(function(i) {

            getThumb($(this).attr('href') || $(this).attr('data-src'), $(this).find('img').attr('src'), i);

        });

        $object.dataL.$cont.find('.rwd-thumb').html(thumbList);

        $thumb = $object.dataL.$cont.find('.rwd-thumb-item');

        $thumb.each(function() {
            var $this = $(this);
            var vimeoVideoId = $this.attr('data-vimeo-id');

            if (vimeoVideoId) {
                $.getJSON('//www.vimeo.com/api/v2/video/' + vimeoVideoId + '.json?callback=?', {
                    format: 'json'
                }, function(data) {
                    $this.find('img').attr('src', data[0]['thumbnail_small']);
                });
            }
        });

        $thumb.eq($object.dataL.index).addClass('active');
        $object.dataL.$element.on('onBeforeSlide.rwd-container', function() {
            $thumb.removeClass('active');
            $thumb.eq($object.dataL.index).addClass('active');
        });

        $thumb.on('click.rwd-container touchend.rwd-container', function() {
            var _$this = $(this);
            setTimeout(function() {
                if ($object.activatedThumbs || !$object.dataL.effectsSupport()) {
                    $object.dataL.index = _$this.index();
                    $object.dataL.slide($object.dataL.index, false, true);
                    $('.rwd-thumb').removeClass('thumb_move');
                }
            }, 50);
        });

        $object.dataL.$element.on('onBeforeSlide.rwd-container', function() {
            $object.animateThumb($object.dataL.index);
        });

        $(window).on('resize.rwd-container.thumb orientationchange.rwd-container.thumb', function() {
            setTimeout(function() {
                $object.animateThumb($object.dataL.index);
                $object.thumbContWidth = $object.$thumbCont.width();
            }, 200);
        });

    };

    Modul.prototype.animateThumb = function(index) {
        var $thumb = this.dataL.$cont.find('.rwd-thumb'),
            position = (this.thumbContWidth / 2) - (this.dataL.modulSettings.thumbsWidth / 2);

        this.left = ((this.dataL.modulSettings.thumbsWidth + this.dataL.modulSettings.thumbMargin) * index - 1) - position;
        if (this.left > (this.thumbTotalWidth - this.thumbContWidth)) {
            this.left = this.thumbTotalWidth - this.thumbContWidth;
        }

        if (this.left < 0) {
            this.left = 0;
        }

        if (!$thumb.hasClass('on')) {
            this.dataL.$cont.find('.rwd-thumb').css('transition-duration', this.dataL.modulSettings.speed + 'ms');
        }

        if (!this.dataL.effectsSupport()) {
            $thumb.animate({
                left: -this.left + 'px'
            }, this.dataL.modulSettings.speed);
        }

        if(!$('.rwd-thumb').hasClass('thumb_move')){
            this.dataL.$cont.find('.rwd-thumb').css({
                transform: 'translate3d(-' + (this.left) + 'px, 0px, 0px)'
            });
        }
    };

    Modul.prototype.enableThumbDrag = function() {

        var $object = this,
            startCoords = 0,
            endCoords = 0,
            isDraging = false,
            isMoved = false,
            tempLeft = 0,
            $left_ = ((this.dataL.modulSettings.thumbsWidth + this.dataL.modulSettings.thumbMargin) * $object.dataL.index - 1) - (this.thumbContWidth / 2) - (this.dataL.modulSettings.thumbsWidth / 2);

        $('.rwd-thumb').attr('data-left', $left_);

        $object.dataL.$cont.find('.rwd-thumb').on('mousedown.rwd-container.thumb', function(e) {
            if ($object.thumbTotalWidth > $object.thumbContWidth) {
                e.preventDefault();
                startCoords = e.pageX;
                isDraging = true;

                $object.dataL.$cont.scrollLeft += 1;
                $object.dataL.$cont.scrollLeft -= 1;

                $object.activatedThumbs = false;
            }
        });

        $(window).on('mousemove.rwd-container.thumb', function(e) {
            if (isDraging) {
                tempLeft = +$('.rwd-thumb').attr('data-left');                isMoved = true;
                endCoords = e.pageX;

                if(Math.abs(endCoords - startCoords) > 0 && $('.rwd-cont').hasClass('rwd-show-autoplay')){
                    $('.rwd-thumb').addClass('thumb_move');
                }

                tempLeft = tempLeft - (endCoords - startCoords);

                if (tempLeft > ($object.thumbTotalWidth - $object.thumbContWidth)) {
                    tempLeft = $object.thumbTotalWidth - $object.thumbContWidth;
                }

                if (tempLeft < 0) {
                    tempLeft = 0;
                }

                $object.dataL.$cont.find('.rwd-thumb').css({
                    transform: 'translate3d(-' + (tempLeft) + 'px, 0px, 0px)'
                });
            }
        });

        $(window).on('mouseup.rwd-container.thumb', function() {
            if (isMoved) {
                isMoved = false;

                $('.rwd-thumb').attr('data-left', tempLeft);

            } else {
                $object.activatedThumbs = true;
            }

            if (isDraging) {
                isDraging = false;
            }
        });

    };

    Modul.prototype.destroy = function () {
        var $object = this;

        $(window).off('.rwd-container.zoom');
        $('.rwd-cont').removeClass('rwd-zoomed');
        clearTimeout($object.zoomabletimeout);
        $object.zoomabletimeout = false;

        if (hugeit_resp_lightbox_obj.hugeit_lightbox_thumbs === 'true') {
            $(window).off('resize.rwd-container.thumb orientationchange.rwd-container.thumb keydown.rwd-container.thumb');
            $('.rwd-cont').removeClass('rwd-has-thumb');
            $('.cont-inner').css({
                height: '100%'
            });
        }

        $object.exitFullscreen();
        $(document).off('fullscreenchange.rwd-container webkitfullscreenchange.rwd-container mozfullscreenchange.rwd-container MSFullscreenChange.rwd-container');
    };

    $.fn.lightbox.lightboxModul.modul = Modul;

    var WaterMark = function (element) {
        this.element = element;
        this.settings = $.extend({}, this.constructor.defaults);
        this.init();
    };

    WaterMark.defaults = {
        imgSrc: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_img_src_new,
        text: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_text,
        textColor: '#' + hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_textColor,
        textFontSize: +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_textFontSize,
        containerBackground: hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_container_bg_color,
        containerWidth: +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_containerWidth,
        position: 'pos' + hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_position_new,
        opacity: +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_opacity / 100,
        margin: +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_margin,
        done: function (imgURL) {
            this.dataset.src = imgURL;
        }
    };

    WaterMark.prototype.init = function () {
        var $object = this,
            $elem = $object.element,
            $settings = $object.settings,
            wmData = {},
            imageData = {};

        var WatermarkImage = jQuery('<img />');
        WatermarkImage.attr('src', $object.settings.imgSrc);
        WatermarkImage.css('display', 'none').attr('id', 'huge_it_watermark_img_sample');
        if (!jQuery('body').find('#huge_it_watermark_img_sample').length) {
            jQuery('body').append(WatermarkImage);
        }

        wmData = {
            imgurl: $settings.imgSrc,
            type: 'jpeg'
        };

        imageData = {
            imgurl: $elem.dataset.imgsrc
        };

        var defer = $.Deferred();

        $.when(defer).done(function (imgObj) {
            imageData.$wmObject = imgObj;

            $object.imgurltodata(imageData, function (dataURL) {
                $settings.done.call($elem, dataURL);
            });
        });

        if ($settings.text !== '') {
            wmData.imgurl = $object.textwatermark();
        }

        $object.imgurltodata(wmData, function (imgObj) {
            defer.resolve(imgObj);
        });
    };

    WaterMark.prototype.textwatermark = function () {
        var $object = this,
            $settings,
            canvas,
            context,
            $width,
            $height;

        $settings = $object.settings;
        canvas = document.createElement('canvas');
        context = canvas.getContext('2d');

        $width = $settings.containerWidth;
        $height = $settings.textFontSize;

        canvas.width = $width;
        canvas.height = $height;

        context.fillStyle = $settings.containerBackground;
        context.fillRect(0, 0, $width, $height);

        context.fillStyle = $settings.textColor;
        context.textAlign = 'center';
        context.font = '500 ' + $settings.textFontSize + 'px Sans-serif';

        context.fillText($settings.text, ($width / 2), ($height - 5));

        return canvas.toDataURL();
    };

    WaterMark.prototype.imgurltodata = function (data, callback) {
        var $object = this,
            $settings = $object.settings,
            img;

        img = new Image();
        img.setAttribute('crossOrigin', 'anonymous');
        img.onload = function () {

            var canvas = document.createElement('canvas'),
                context = canvas.getContext('2d'),

                $imgWidth = this.width,
                $imgHeight = this.height;

            if (data.$wmObject) {

                if (data.width !== 'auto' && data.height === 'auto' && data.width < $imgWidth) {
                    $imgHeight = $imgHeight / $imgWidth * data.width;
                    $imgWidth = data.width;
                } else if (data.width === 'auto' && data.height !== 'auto' && data.height < $imgHeight) {
                    $imgWidth = $imgWidth / $imgHeight * data.height;
                    $imgHeight = data.height;
                } else if (data.width !== 'auto' && data.height !== 'auto' && data.width < $imgWidth && data.height < $imgHeight) {
                    $imgWidth = data.width;
                    $imgHeight = data.height;
                }

            }


            canvas.width = $imgWidth;
            canvas.height = $imgHeight;

            /*if (data.type === 'jpeg') {
             context.fillStyle = '#ffffff';
             context.fillRect(0, 0, $imgWidth, $imgHeight);
             }*/

            context.drawImage(this, 0, 0, $imgWidth, $imgHeight);

            if (data.$wmObject) {

                var $opacity = +hugeit_gen_resp_lightbox_obj.hugeit_lightbox_watermark_containerOpacity / 100;
                if ($opacity >= 0 && $opacity <= 1) {
                    //context.globalAlpha = $settings.opacity;
                    context.globalAlpha = $opacity;
                }

                var $wmWidth,
                    $wmHeight,
                    pos = $settings.margin,
                    $x, $y;
                if ($settings.text !== '') {
                    $wmWidth = data.$wmObject.width;
                    $wmHeight = data.$wmObject.height;
                }
                else {
                    $wmWidth = $settings.containerWidth;
                    $wmHeight = (jQuery('img#huge_it_watermark_img_sample').prop('naturalHeight') * $wmWidth) / jQuery('img#huge_it_watermark_img_sample').prop('naturalWidth');
                }

                switch ($settings.position) {
                    case 'pos1':
                        $x = pos;
                        $y = pos;
                        break;
                    case 'pos2':
                        $x = $imgWidth / 2 - $wmWidth / 2;
                        $y = pos;
                        break;
                    case 'pos3':
                        $x = $imgWidth - $wmWidth - pos;
                        $y = pos;
                        break;
                    case 'pos4':
                        $x = pos;
                        $y = $imgHeight / 2 - $wmHeight / 2;
                        break;
                    case 'pos5':
                        $x = $imgWidth / 2 - $wmWidth / 2;
                        $y = $imgHeight / 2 - $wmHeight / 2;
                        break;
                    case 'pos6':
                        $x = $imgWidth - $wmWidth - pos;
                        $y = $imgHeight / 2 - $wmHeight / 2;
                        break;
                    case 'pos7':
                        $x = pos;
                        $y = $imgHeight - $wmHeight - pos;
                        break;
                    case 'pos8':
                        $x = $imgWidth / 2 - $wmWidth / 2;
                        $y = $imgHeight - $wmHeight - pos;
                        break;
                    case 'pos9':
                        $x = $imgWidth - $wmWidth - pos;
                        $y = $imgHeight - $wmHeight - pos;
                        break;
                    default:
                        $x = $imgWidth - $wmWidth - pos;
                        $y = $imgHeight - $wmHeight - pos;
                }
                context.drawImage(data.$wmObject, $x, $y, $wmWidth, $wmHeight);
            }

            var dataURL = canvas.toDataURL('image/' + data.type);

            if (typeof callback === 'function') {

                if (data.$wmObject) {
                    callback(dataURL);

                } else {
                    var $wmNew = new Image();
                    $wmNew.src = dataURL;
                    callback($wmNew);
                }
            }

            canvas = null;
        };

        img.src = data.imgurl;
    };

    $.fn['watermark'] = function () {
        return this.each(function () {
            if (!$.data(this, 'watermark')) {
                $.data(this, 'watermark', new WaterMark(this));
            }
        });
    };

})(jQuery);
