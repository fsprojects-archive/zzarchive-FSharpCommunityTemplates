$(function () {
    appFsMvc.ContactDetailsView = Backbone.View.extend({
        events: {
            "click #createContact": "gotoCreateView"
        },

        render: function () {
            appFsMvc.utility.renderTemplate("contactDetail.htm", $(this.el), { data: this.model.toJSON() });
        },

        gotoCreateView: function () {
            appFsMvc.App.navigate( "create", { trigger: true } );
        }
    });
});