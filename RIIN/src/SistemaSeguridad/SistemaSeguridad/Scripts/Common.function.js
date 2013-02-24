/*****************

*****************/
function formatDate(data, format) {
    var meses;

    if (format != undefined && format == 1) {
        meses = new Array("01", "02", "03",
            "04", "05", "06", "07", "08", "09",
            "10", "11", "12");

    }
    else {
        meses = new Array("Enero", "Febrero", "Marzo",
            "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre",
            "Octubre", "Noviembre", "Diciembre");
    }
    var fecha = data.getDate() + '/' + meses[data.getMonth()] + '/' + data.getFullYear();
    return fecha;
}

function getJson(url, data, successfunction) {
    $.getJSON(url, JSON.stringify(data), function(result, status, xhr) {
        if (status == 'success') {
            if (successfunction != undefined || successfunction != null)
                successfunction(result);
        } else {
            LogPostError(xhr, status);
        }
        var htmlResult = $(result).find("div#content");
        if (htmlResult != null) {
            $("#content").html(htmlResult);
        }
    });
}

function postJson(id, successfunction) {
    var formulario = $("#" + id);
    $.post(formulario.attr('action'), formulario.serialize(), function (result, status, xhr) {
        if (status == "success") {
            if (successfunction != undefined || successfunction != null)
                successfunction(result);
        } else {
            LogPostError(xhr, status);
        }
//        var htmlResult = $(result).find("div#content");
//        if (htmlResult != null) {
//            $("#content").html(htmlResult);
//        }
    });
}
function formato(object, size) {
    $(object).multiselect({
        multiple: false,
        selectedList: 4,
        header: false,
        minWidth: size,
        noneSelectedText: "Seleccione..."
    });
    $(object).multiselect('refresh');
}

function multiseleccion(object, size) {
    $(object).multiselect({
        selectedList: 4,
        header: false,
        minWidth: size,
        noneSelectedText: "Seleccione..."
    });
    $(object).multiselect('refresh');
}
        
function saveForm(id, idMessage, successfunction) {
    //$('form').live('submit', function (e) {
    $('input[type=submit]').attr('disabled', true);
    if (successfunction != undefined || successfunction != null) {
        postJson(id, function () {
            successfunction();
            $(this).each(function () { this.reset(); });
            $('input[type=submit]').attr('disabled', true);
        });
    } else {
        postJson(id, function () {
            $(this).each(function () { this.reset(); });
            $('input[type=submit]').attr('disabled', true);
        });
    }
}

function confirmDelete(url, idElementDelete, successfunction) {
    // $('.delete').click(function() {$(this).attr('id')
    $.post(url, { id: idElementDelete }, function (result, status, xhr) {
        if (status == "success") {
            if (successfunction != undefined || successfunction != null)
                successfunction(result);
        } else {
            LogPostError(xhr, status);
        }
    });
}

function OpenPopUpVariable(id, modalWidth, modalHeight, modalTitle) {
    var answer;
    $("#" + id + ":ui-dialog").dialog("destroy");
    $("#" + id).dialog({
        width: modalWidth,
        height: modalHeight,
        title: modalTitle,
        modal: true,
        closeOnEscape: false,
        resizable: false,
        buttons: {
            "Si": function() {
                $(this).dialog("close");
                OcultarDivIncidente();
                answer = true;
            },
            "No": function() {
                $(this).dialog("close");
                OcultarDivIncidente();
                answer = false;
            }
        }
    });
    return answer;
}

function OcultarDivIncidente() {
    var incidenteActivo = $('#divTipoIncidente span').text();
    obj = document.getElementById(incidenteActivo);
    obj.style.display = "none";    
}

function OpenPopUp(config) {
        var showDialog = $("#" + config.id);
        var buttons = "";

    var defaulConfig = {
        height: 140,
        width: 480,
        buttons: 2
    };

    var Config = $.extend(defaulConfig, config);
    
       switch (Config.buttons) {
            case 1:
                buttons = {
                    "Ok": function () {
                        Config.ok();
                        showDialog.dialog("close");
                    }
                };
                break;
            default:
                buttons = {
                    "Si": function () {
                        Config.yes();
                        showDialog.dialog("close");
                    },
                    "No": function () {
                        Config.no();
                        showDialog.dialog("close");
                    }
                };
                break;
        }

    showDialog.dialog("open");
    showDialog.dialog({
        height: Config.height,
        width: Config.width,
        modal: true,
        resizable: false,
        buttons: buttons
    });
}

function LogPostError(xhr, status) {
    var error = "";
    switch (status) {
        case 'undefined':
            return;
            break;
        case 'error':
            switch (xhr.status) {
                case 0:
                    return;
                    break;
                case 401:
                case 404:
                    error = 'HTTP ' + xhr.status + 'Error\n' + xhr.name;
                    break;
                case 500:
                    error = 'HTTP ' + xhr.status + 'Internal Server Error in ' + xhr.name;
                    break;
                default:
                    error = 'HTTP ' + xhr.status + 'Unknow Error\n' + +xhr.name;
                    break;
            }
            break;
        case 'timeout':
            error = 'Timeout. The server was taking too long to respond. Please try again in a few moments. ' + xhr.name;
            break;
        case 'parsererror':
            error = 'Error.\nCould Not Process Server Response. ';
            break;
        default:
            error = 'Unexpected HTTP Error.\n ' + error + ' ' + status;
            break;
    }
}

function RemoveInvalidChars(strValue, invalidChars) {
    var iChars;
    if (invalidChars == null) {
        iChars = "0123456789abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚÜáéíóúü¡!@#$%^&*()+=-_[]\\\';,./{}|\":<>¿?~`" + String.fromCharCode(10, 13, 32);
    } else {
        iChars = invalidChars;
    }
    for (var i = 0; i < strValue.length; i++) {
        if (iChars.indexOf(strValue.charAt(i)) >=0 ) {
            strValue = strValue.replace(strValue.substr(i, 1), '');
        }
    }
    return strValue;
}


function isValidateUrlField(fieldId) {
    var invalidChars = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ$!¡,(;@_-' + String.fromCharCode(32);
    return isValidText(fieldId, invalidChars);
}

function isValidText(fieldId, invalidChars) {
    var field = $("#" + fieldId);
    var iChars;
    if (invalidChars == null) {
        iChars = "0123456789abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚÜáéíóúü¡!@#$%^&*()+=-_[]\\\';,./{}|\":<>¿?~`" + String.fromCharCode(10, 13, 32);
    } else {
        iChars = invalidChars;
    }
    for (var i = 0; i < field.val().length; i++) {
        if (iChars.indexOf(field.val().charAt(i)) == -1) {
            return false;
        }
    }
    return true;
}
