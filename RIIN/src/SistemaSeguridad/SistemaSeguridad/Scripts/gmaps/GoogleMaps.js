/*
//
*/
function GoogleMaps() {
    var map;
    var marker;
    var oneMarkerOnMap;
    var automaticCenterMap;
    var addMarkerOnFindAddress;
    var geocoder = new google.maps.Geocoder();
    
    function loadScript() {
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.src = "http://maps.google.com/maps/api/js?sensor=false";
        document.getElementsByTagName('head')[0].appendChild(script);
    }

    this.initialize = function (mapId) {
        map = new google.maps.Map(document.getElementById(mapId), {
            zoom: 10,
            center: new google.maps.LatLng(25.706506, -100.117035),
            draggableCursor: 'default',
            draggingCursor: 'pointer',
            mapTypeControl: true,
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
            },
            navigationControlOptions: {
                style: google.maps.NavigationControlStyle.ZOOM_PAN
            },
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        return map;
    };

    this.findAddress = function (address, returnFunction, googleMap) {
        if (geocoder) {
            geocoder.geocode({ 'address': address }, function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (googleMap == undefined) {
                        map.setCenter(results[0].geometry.location);
                        if (addMarkerOnFindAddress) {
                            addMarker(results[0].geometry.location);
                        }
                    } else {
                        googleMap.setCenter(results[0].geometry.location, 15);
                        //googleMap.setOptions({ zoom: 15 });
                    }
                    setZoom(15);
                    if (returnFunction != undefined || returnFunction != null)
                        returnFunction(results[0].address_components);
                }else {
                    alert("Geocode was not successful for the following reason: " + status);
                }
            });
        }
    };

    this.reverseGeocoder = function (latLng, returnFunction) {
        // finds the address for the given location
        var latlngStr = latLng.split(",", 2);
        var lat = parseFloat(latlngStr[0]);
        var lng = parseFloat(latlngStr[1]);
        var latlng = new google.maps.LatLng(lat, lng);
        var infowindow = new google.maps.InfoWindow();

        var geocoder = new google.maps.Geocoder();
        if (geocoder) {
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        address = results[0].formatted_address;
                        // fill in the results in the form
                        //                        marker = new google.maps.Marker({
                        //                            position: latlng,
                        //                          /// <reference path="../../Views/Zona/" />
                        //map: map
                        //                        });
                        //                        infowindow.setContent(results[1].formatted_address);
                        //                        infowindow.open(map, marker);
                        if (returnFunction != undefined) {
                            returnFunction(results[0].address_components);
                        }
                    }
                }
                else {
                    alert("Geocoder failed due to: " + status);
                }
            });
        }
    };

    this.centerMap = function (lat, lng) {
        var position = new google.maps.LatLng(lat, lng);
        map.setCenter(position);
    };
    this.clearMap = function () {
        if (marker != undefined && oneMarkerOnMap)
            marker.setMap(null);
    };
    
    this.addListener = function (listener, returnFunction) {
        google.maps.event.addListener(map, listener, function (event) {
            addMarker(event.latLng);
            if (returnFunction != undefined)
                returnFunction(event.latLng);
        });
    };

    function addMarker(location) {
        if (marker != undefined && oneMarkerOnMap)
            marker.setMap(null);

        marker = new google.maps.Marker({
            position: location,
            map: map
        });
    };
    
    function setZoom(value) {
        map.setOptions({
            zoom: value
        });

    }

    this.addMarker = function (lat, lng, returnfunction) {
        var position = new google.maps.LatLng(lat, lng);
        addMarker(position);
        if (automaticCenterMap) {
            map.setCenter(position);
            setZoom(15);
        }
        if (returnfunction != undefined) {
            returnfunction(position);
        }
    };

    this.singleMarkerOnMap = function (value) {
        oneMarkerOnMap = value;
    };

    this.automaticCenterMapOnMarkerAdded = function (value) {
        automaticCenterMap = value;
    };

    this.AddMarkerOnFindAddress = function (value) {
        addMarkerOnFindAddress = value;
    };
}
