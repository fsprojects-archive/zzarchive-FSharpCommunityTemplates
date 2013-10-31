$(function () {
    var AppRouter = Backbone.Router.extend({

        routes: {
            "": "list",
            "create": "create"
        },

        initialize: function () {
            this.contactDetailsView = new appFsMvc.ContactDetailsView( { el: $("#content"), model: window.appFsMvc.contacts } );
            this.createContactView = new appFsMvc.ContactCreateView( { el: $("#content"), model: window.appFsMvc.contacts } );            
        },

        list: function () {
            this.contactDetailsView.render();
        },

        create: function () {
            this.createContactView.render();
        }
    });

    appFsMvc.contacts = new appFsMvc.ContactCollection();
    $.getJSON( appFsMvc.contacts.url, function ( data ) {
        appFsMvc.contacts.reset( data );
        appFsMvc.App = new AppRouter();
        Backbone.history.start();
    });
});
