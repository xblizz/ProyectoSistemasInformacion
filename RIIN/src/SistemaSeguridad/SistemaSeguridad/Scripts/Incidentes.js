

//Id Nombre
//1	Robo con violencia                                
//2	Robo sin violencia                                
//3	Secuestro de empleado en ruta                     
//4	Extorsión                                         
//5	Amenaza                                           
//6	Intrusión
//7	Otro

function GeneraPanelIncidente(tipo) {
    var obj;
    var cmbTipoIncidente = document.getElementById("ddTipoIncidente");

    var titulo = document.getElementById("TituloIncidente");
//    if (cmbTipoIncidente.options.counter)
//        titulo.innerHTML = "Tipo de incidente: " + cmbTipoIncidente.options[cmbTipoIncidente.selectedIndex].text;

//    var divs = $('div[id="CamposEspecificos"] > div');
//    var currentElement;

//    $.each(divs, function (i, item) {
//        currentElement = divs[i];
//        $(currentElement).attr("class", "DivOculto");
//        i++;
//    });

   

    function formato(object) {
        $(object).multiselect({
            multiple: false,
            selectedList: 4,
            header: false,
            noneSelectedText: "Seleccione..."
        });
        $(object).multiselect('refresh');
    }



    switch (tipo) {
        case "1":
            $("#TipoArmaId").Cascade("/Helper/GetTiposArma", null, formato);
            $("#ddCantidadDelincuentes").Cascade("/Helper/GetDelincuentes", null, formato);
            $("#ddTipoVehiculo").Cascade("/Helper/GetTiposVehiculo", null, formato);
            break;

        case "3":
            $("#TipoArmaId").Cascade("/Helper/GetTiposArma", null, formato);
            $("#ddCantidadDelincuentes").Cascade("/Helper/GetDelincuentes", null, formato);
            $("#ddTipoVehiculo").Cascade("/Helper/GetTiposVehiculo", null, formato);
            break;
        case "4":
            $("#TipoExtorsionId").Cascade("/Helper/GetTiposExtorcion", null, formato);
            break;
        case "5":
            $("#MedioAmenazaId").Cascade("/Helper/GetTiposAmenaza", null, formato);
            $("#MedioAmenazaId").Cascade("/Helper/GetMediosAmenaza", null, formato);
            break;
        case "6":
            $("#TipoIntrusionId").Cascade("/Helper/GetTiposIntrusion", null, formato);
            break;
        default:
            obj = document.getElementById("TI_Otros");
            $(obj).attr("class", "DivVisible");
            break;
    }
}

