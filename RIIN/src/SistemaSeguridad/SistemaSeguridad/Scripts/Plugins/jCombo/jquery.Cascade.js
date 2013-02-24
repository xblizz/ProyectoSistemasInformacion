/*!
 * jQuery Cascade Plugin
 * Carlos De Oliveira
 * cardeol@gmail.com
 *
 * Latest Release: Sep 2011 
 */
(function ($) {
    $.fn.Cascade = function (url, user_options, formatFunction, cmbSize) {
        var default_options = {
            parent: "",
            selected_value: "0",
            parent_value: "",
            initial_text: ""
        };
        var user_options = $.extend(default_options, user_options);
        var obj = $(this);
        if (cmbSize == undefined || cmbSize <= 0)
            cmbSize = 160;
        if (user_options.parent != "") {
            var $parent = $(user_options.parent);
            $parent.removeAttr("disabled", "disabled");
            $parent.bind('change', function (e) {
                obj.attr("disabled", "disabled");
                if ($(this).val() != "0" && $(this).val() != "") obj.removeAttr("disabled");
                __fill(obj,
                    url,
                    $(this).val(),
                    user_options.initial_text,
                    user_options.selected_value, cmbSize);
            });
        }
        __fill(obj, url, user_options.parent_value, user_options.initial_text, user_options.selected_value, cmbSize);


        function __fill($obj, $url, $id, $initext, $inival, $cmbSize) {
            $.ajax({
                type: "GET",
                dataType: "json",
                url: $url,
                data: { filterId: $id },
                success: function (j) {
                    var choices = '';
                    if (j.length == 0) {
                        //choices += '<option value="0">Seleccione...</option>';
                        choices = '';
                        $obj.html(choices);
                    } else {
                        if ($initext != "" && $initext != null) {
                            choices += '<option value="0">' + $initext + '</option>';
                        }
                        for (var i = 0; i < j.length; i++) {
                            c = j[i];
                            selected = (c.Id == $inival) ? ' selected="selected"' : '';
                            
                            choices += '<option value="' + c.Id + '"' +
                                selected + '>' + c.Descripcion +
                                    '</option>';
                        }
                        $obj.html(choices);
                    }
                    if (formatFunction != undefined)
                        formatFunction(obj, $cmbSize);
                    $obj.trigger("change");
                }
            });
        }
    };
})(jQuery);