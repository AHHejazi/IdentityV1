//GoogleMapWidget class constructor
GoogleMapWidget = function (mapSettings) //constructor for the proxy
{
    //initialize class logic here

    this.mapSettings = mapSettings;

    this.container = $('#' + mapSettings.containerId);
    this.mapContainer = $('#' + mapSettings.containerId + ' .google-map');
    this.txtLocation = $('#' + mapSettings.containerId + ' .google-map-input');

    this.infowindow = null;
    this.map = null;
    this.marker = null;
};
//GoogleMapWidget class
GoogleMapWidget.prototype =
    {
        //widget load entry point
    load: function (isForShow) {

            var self = this; //to avoid this inside jquery ajax success . as there refers to ajax obj

            if (!self.container.length) return;

            var center = new google.maps.LatLng(self.mapSettings.centerLatitude, self.mapSettings.centerLongitude, true);

            // in edit mode if text box has value set center and maker pos to the text box value
            if (self.txtLocation.val()) {
                var val = $.trim(self.txtLocation.val());
                center = new google.maps.LatLng(val.split(',')[0], val.split(',')[1]);
            }

            //set the icon image and att
            //var iconImg = new google.maps.MarkerImage(self.ImageUrl, new google.maps.Size(32, 37), null, null);

            self.map = new google.maps.Map(self.mapContainer.get(0), {
                zoom: self.mapSettings.zoomLevel,
                center: center,
                scrollwheel: true,
                navigationControl: true,
                mapTypeControl: true,
                draggable: true,
                scaleControl: true,
                streetViewControl: false,
                panControl: true,
                zoomControl: true,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                backgroundColor: '#fff',
                draggableCursor: 'move'//,
                //minZoom: 1,
                //maxZoom: 19
            });

            self.marker = new google.maps.Marker({ //init
                map: self.map,
                draggable: true,
                //icon: iconImg,
                zIndex: 1,
                animation: google.maps.Animation.DROP
            });

            self.marker.setPosition(center);
            self.marker.setVisible(true);

            //Map events
            if (isForShow != "False") {
                google.maps.event.addListener(self.marker, 'drag', function () {
                    self.updateLocationInput(self.marker.position);
                });

                google.maps.event.addListener(self.marker, 'click', function () {
                    self.updateLocationInput(self.marker.position);
                });

                google.maps.event.addListener(self.map, 'click', function (event) {

                    self.updateLocationInput(event.latLng);
                    self.marker.setPosition(event.latLng);
                });
            }


            // if($('[data-toggle="collapse"]').hasClass('mapclickeventattached') === false)
            //  {$('[data-toggle="tab"]')

            //    $('[data-toggle="collapse"]').addClass('mapclickeventattached');

            $('[data-toggle="collapse"], [data-toggle="tab"]').click(function () {


                window.setTimeout(function () {

                    var visibleMaps = $('#' + self.mapSettings.containerId + ':visible');

                    if (visibleMaps && visibleMaps.length) {
                        google.maps.event.trigger(self.map, 'resize');
                        if (self.marker.position)
                            self.map.setCenter(self.marker.position);
                        else
                            self.map.setCenter(center);
                    }
                }, 150);

            });

            //   }

            self.txtLocation.keydown(function (e) {
                var val = $.trim(self.txtLocation.val());

                if (val && val.length && val.indexOf(',') > 0) {

                    var isMatch = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/.test(val);

                    if (isMatch) {
                        var myLatlng = new google.maps.LatLng(val.split(',')[0], val.split(',')[1]);
                        self.map.setCenter(myLatlng);
                        self.marker.setPosition(myLatlng);
                    }
                }
            });
        }, //load End.

        updateLocationInput: function (pos) {
            var self = this;
            var latLonVal = pos.lat().toString().substring(0, 8) + ", " + pos.lng().toString().substring(0, 8);
            self.txtLocation.val(latLonVal);
            //if selected value changed fire validation if validation library exist
            if (self.txtLocation.valid) {
                self.txtLocation.valid();
            }
        }

    };//GoogleMapWidget class end

