/*
//
*/
function PolygonControl(idmap) {
    var idMap = idmap;
    var polyShape;
    var startMarker;
    var markers = [];
    var midmarkers = [];
    var pointsArray = [];
    var editing = false;
    var polyPoints = [];
    var map;

    var imageNormal = new google.maps.MarkerImage(
	    "http://www.birdtheme.org/useful/images/square.png",
	    new google.maps.Size(11, 11),
	    new google.maps.Point(0, 0),
	    new google.maps.Point(6, 6)
    );
    var imageHover = new google.maps.MarkerImage(
	    "http://www.birdtheme.org/useful/images/square_over.png",
	    new google.maps.Size(11, 11),
	    new google.maps.Point(0, 0),
	    new google.maps.Point(6, 6)
    );
    var imageNormalMidpoint = new google.maps.MarkerImage(
	    "http://www.birdtheme.org/useful/images/square_transparent.png",
	    new google.maps.Size(11, 11),
	    new google.maps.Point(0, 0),
	    new google.maps.Point(6, 6)
    );

    this.preparePolygon = function () {
        if (polyPoints == null) { polyPoints = new google.maps.MVCArray(); }
        polyShape = new google.maps.Polygon({
            path: polyPoints,
            strokeColor: "#FF0000",
            strokeOpacity: 1,
            strokeWeight: 2,
            fillColor: "#FF0000",
            fillOpacity: 0.4
        });
        if (polyPoints.length == 0) { map = new GoogleMaps().initialize(idMap); };
        polyShape.setMap(map);
        var conten = "<b>Nombre del Polygono<b/><br/> <input class='text-box single-line' type='text' value='' name='Nombre' />";
        attachPolygonInfoWindow(polyShape, conten);
        return map;
    };

    this.loadPolygon = function (cords) {
        polyShape = new google.maps.Polygon({ 
            path: cords,
            strokeColor: "#FF0000",
            strokeOpacity: 1,
            strokeWeight: 2,
            fillColor: "#FF0000",
            fillOpacity: 0.4
        });
        if (polyPoints.length == 0) { map = new GoogleMaps().initialize(idMap); };
        polyShape.setMap(map);
        return map;
    };
    
    function attachPolygonInfoWindow(polygon, html)
    {
	    polygon.infoWindow = new google.maps.InfoWindow({
		    content: html
	    });
	    google.maps.event.addListener(polygon, 'click', function(e) {
		    var latLng = e.latLng;
		    this.setOptions({fillOpacity:0.1});
		    polygon.infoWindow.setPosition(latLng);
		    polygon.infoWindow.open(map);
	    });
//	    google.maps.event.addListener(polygon, 'mouseout', function() {
//		    this.setOptions({fillOpacity:0.35});
//		    polygon.infoWindow.close();
//	    });
    }

    this.addLatLng = function (point) {
        polyPoints = polyShape.getPath();
        polyPoints.insertAt(polyPoints.length, point.latLng); // or: polyPoints.push(point.latLng)
        var stringtobesaved = point.latLng.lat().toFixed(6) + ',' + point.latLng.lng().toFixed(6);
        pointsArray.push(stringtobesaved);
        logCode(); //  // write latlong Polygon
    };

    // write latlong for polygon in div hidden
    function logCode() {
        var geocode = $("#geocoordinates");
        geocode.empty();
        $.each(pointsArray, function (i, item) {
            geocode.append('<input type="hidden" name="Coords[' + i + ']" value="' + item + '"  />');
        });
    }

    this.setTool = function () {
        if (this.polyShape) this.polyShape.setMap(null);
        //this.preparePolygon();
        logCode();
    };

    this.clearMap = function () {
        if (editing) { stopediting(); };
        if (startMarker) startMarker.setMap(null);
        polyShape.setMap(null);
        polyPoints = [];
        pointsArray = [];
        $("#geocoordinates").empty();
        this.preparePolygon();
        google.maps.event.addListener(map, 'click', this.addLatLng);
    };

    function stopediting() {
        editing = false;
        $('#btnEdit').val('Edit lines');
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        for (var index = 0; index < midmarkers.length; index++) {
            midmarkers[index].setMap(null);
        }
        polyPoints = polyShape.getPath();
        markers = [];
        midmarkers = [];
    }

    // the "Edit lines" button has been pressed
    this.editlines = function () {
        if (editing == true) {
            stopediting();
        } else {
            polyPoints = polyShape.getPath();
            if (polyPoints.length > 0) {
                this.setTool();
                for (var i = 0; i < polyPoints.length; i++) {
                    var marker = setmarkers(polyPoints.getAt(i));
                    markers.push(marker);
                    if (i > 0) {
                        var midmarker = setmidmarkers(polyPoints.getAt(i));
                        midmarkers.push(midmarker);
                    }
                }
                editing = true;
                $('#btnEdit').val('Stop edit');
            }
        }
    };
    
    function setmarkers(point){
        var marker = new google.maps.Marker({
            position: point,
            map: map,
            icon: imageNormal,
            raiseOnDrag: false,
            draggable: true
        });
        google.maps.event.addListener(marker, "mouseover", function () {
            marker.setIcon(imageHover);
        });
        google.maps.event.addListener(marker, "mouseout", function () {
            marker.setIcon(imageNormal);
        });
        google.maps.event.addListener(marker, "drag", function () {
            for (var i = 0; i < markers.length; i++) {
                if (markers[i] == marker) {
                    polyShape.getPath().setAt(i, marker.getPosition());
                    movemidmarker(i);
                    break;
                }
            }
            polyPoints = polyShape.getPath();
            var stringtobesaved = marker.getPosition().lat().toFixed(6) + ',' + marker.getPosition().lng().toFixed(6);
            pointsArray.splice(i, 1, stringtobesaved);
            logCode();
        });

        google.maps.event.addListener(marker, "click", function () {
            for (var i = 0; i < markers.length; i++) {
                if (markers[i] == marker && markers.length != 1) {
                    marker.setMap(null);
                    markers.splice(i, 1);
                    polyShape.getPath().removeAt(i);
                    removemidmarker(i);
                    break;
                }
            }
            polyPoints = polyShape.getPath();
            if (markers.length > 0) {
                pointsArray.splice(i, 1);
                logCode();
            }
        });
        return marker;
    }

    function setmidmarkers(point) {
        var prevpoint = markers[markers.length - 2].getPosition();
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(
                    point.lat() - (0.5 * (point.lat() - prevpoint.lat())),
                    point.lng() - (0.5 * (point.lng() - prevpoint.lng()))
             ),
            map: map,
            icon: imageNormalMidpoint,
            raiseOnDrag: false,
            draggable: true,
            title: "Drag me to change shape",
            icon: new google.maps.MarkerImage("http://geojason.info/images/demos/markers/measure-vertex.png", new google.maps.Size(9, 9), new google.maps.Point(0, 0), new google.maps.Point(5, 5))
        });
        google.maps.event.addListener(marker, "mouseover", function () {
            marker.setIcon(imageNormal);
        });
        google.maps.event.addListener(marker, "mouseout", function () {
            marker.setIcon(imageNormalMidpoint);
        });
        google.maps.event.addListener(marker, "dragend", function () {
            var newpos = null;
            for (var i = 0; i < midmarkers.length; i++) {
                if (midmarkers[i] == marker) {
                    newpos = marker.getPosition();
                    var startMarkerPos = markers[i].getPosition();
                    var firstVPos = new google.maps.LatLng(
                        newpos.lat() - (0.5 * (newpos.lat() - startMarkerPos.lat())),
                        newpos.lng() - (0.5 * (newpos.lng() - startMarkerPos.lng()))
    			    );
                    var endMarkerPos = markers[i + 1].getPosition();
                    var secondVPos = new google.maps.LatLng(
                        newpos.lat() - (0.5 * (newpos.lat() - endMarkerPos.lat())),
                        newpos.lng() - (0.5 * (newpos.lng() - endMarkerPos.lng()))
    			    );
                    var newVMarker = setmidmarkers(secondVPos);
                    newVMarker.setPosition(secondVPos); //apply the correct position to the midmarker
                    var newMarker = setmarkers(newpos);
                    markers.splice(i + 1, 0, newMarker);
                    polyShape.getPath().insertAt(i + 1, newpos);
                    marker.setPosition(firstVPos);
                    midmarkers.splice(i + 1, 0, newVMarker);
                    break;
                }
            }
            polyPoints = polyShape.getPath();
            var stringtobesaved = newpos.lat().toFixed(6) + ',' + newpos.lng().toFixed(6);
            pointsArray.splice(i + 1, 0, stringtobesaved);
            logCode();
        });
        return marker;
    }

    function movemidmarker(index) {
        var newpos = markers[index].getPosition();
        if (index != 0) {
            var prevpos = markers[index - 1].getPosition();
            midmarkers[index - 1].setPosition(new google.maps.LatLng(
                newpos.lat() - (0.5 * (newpos.lat() - prevpos.lat())),
                newpos.lng() - (0.5 * (newpos.lng() - prevpos.lng()))
    	    ));
        }
        if (index != markers.length - 1) {
            var nextpos = markers[index + 1].getPosition();
            midmarkers[index].setPosition(new google.maps.LatLng(
                newpos.lat() - (0.5 * (newpos.lat() - nextpos.lat())),
                newpos.lng() - (0.5 * (newpos.lng() - nextpos.lng()))
    	    ));
        }
    }

    function removemidmarker(index) {
        if (markers.length > 0) { //clicked marker has already been deleted
            if (index != markers.length) {
                midmarkers[index].setMap(null);
                midmarkers.splice(index, 1);
            } else {
                midmarkers[index - 1].setMap(null);
                midmarkers.splice(index - 1, 1);
            }
        }
        if (index != 0 && index != markers.length) {
            var prevpos = markers[index - 1].getPosition();
            var newpos = markers[index].getPosition();
            midmarkers[index - 1].setPosition(new google.maps.LatLng(
                newpos.lat() - (0.5 * (newpos.lat() - prevpos.lat())),
                newpos.lng() - (0.5 * (newpos.lng() - prevpos.lng()))
    	    ));
        }
    }
}
