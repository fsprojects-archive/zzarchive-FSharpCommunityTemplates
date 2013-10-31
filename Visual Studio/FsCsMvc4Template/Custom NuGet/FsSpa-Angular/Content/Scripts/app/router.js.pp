(function (util) {    
    angular.module("contactsApp.service", [], function ($provide) {
        $provide.factory("ContactsService", ["$http", "$location", function($http, $location) {
            var contactService = {};
            var contacts = [];

            contactService.getAll = function(callback) {
                if (contacts.length === 0) {
                    $http.get("/api/contacts").success(function(data) {
                        contacts = data;
                        callback(contacts);
                    });
                } else {
                    callback(contacts);
                }
            };

            contactService.addItem = function (item) {
                contacts.push(item);
                $http({
                    url: "/api/contacts",
                    method: "POST",
                    data: JSON.stringify(item),
                })
                .success(function () {
                    toastr.success("You have successfully created a new contact!", "Success!");
                    $location.path("/");
                })
                .error(function () {
                    contacts.pop();
                    toastr.error("There was an error creating your new contact", "<sad face>");
                });
            };

            return contactService;
        }]);
    });
    
    angular.module("contactsapp", ["contactsApp.service"])
        .config(["$routeProvider", function ($routeProvider) {
            $routeProvider
                .when("/create", { templateUrl: util.buildTemplateUrl("contactCreate.htm") })
                .otherwise({ redirectTo: "/", templateUrl: util.buildTemplateUrl("contactDetail.htm") });
        }]);
})(appFsMvc.utility);

