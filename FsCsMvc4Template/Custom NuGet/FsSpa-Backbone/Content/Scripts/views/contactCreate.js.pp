$(function () {
    appFsMvc.ContactCreateView = Backbone.View.extend({
        events: {
            "click #saveContact": "createContact"
        },

        render: function () {
            appFsMvc.utility.renderTemplate( "contactCreate.htm", $(this.el) );
        },

        createContact: function () {
            var data = appFsMvc.utility.serializeObject( $("#contactForm") );
            var that = this;
            
            $.ajax({
                url: this.model.url,
                data: JSON.stringify( data ),
                type: "POST",
                dataType: 'json',
                contentType: 'application/json'
            })
            .done( function () {
                toastr.success( "You have successfully created a new contact!", "Success!" );
                that.model.add( data );
                appFsMvc.App.navigate( "", { trigger: true } );
            })
            .fail( function () {
                toastr.error( "There was an error creating your new contact", "<sad face>" );
            });

            return false;
        }
    });
});