(function (module, $) {
    module.contactCreateController = function ($scope, ContactsService) {
        $scope.addContact = function () {
            var data = appFsMvc.utility.serializeObject($("#contactForm"));
            ContactsService.addItem(data);
        };
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {}, jQuery);