$(function () {
    appFsMvc.Contact = Backbone.Model.extend();

    appFsMvc.ContactCollection = Backbone.Collection.extend({
        model: window.appFsMvc.Contact,
        url: "/api/contacts/"
    });
});