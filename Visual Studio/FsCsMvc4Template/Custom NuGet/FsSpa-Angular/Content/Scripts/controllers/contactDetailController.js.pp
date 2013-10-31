(function (module) {
    module.contactDetailController = function ($scope, ContactsService) {
         ContactsService.getAll(function(data) {
             $scope.contacts = data;
         });
    };
})(appFsMvc.Controllers = appFsMvc.Controllers || {});