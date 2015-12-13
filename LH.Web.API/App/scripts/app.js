'use strict';
// 立即执行函数
(function () {
    var app = angular.module("LH", ['ngRoute', "ui.bootstrap", "LH.Controllers", "LH.Services", "LH.Filters", "LH.Directives"]);
    app.config(['$routeProvider', function ($routeProvider) {
        // 路由配置
        var route = $routeProvider;
        route.when("/users/list", { controller: 'users', templateUrl: '/users-list' });
        route.when("/users/user/", { controller: 'usersDetail', templateUrl: '/users-detail' });
        route.when("/users/user/:id", { controller: 'usersDetail', templateUrl: '/users-detail' });
        route.when("/roles/list", { controller: 'roles', templateUrl: '/roles-list' });
        route.when("/permissions/list", { controller: 'permissions', templateUrl: '/permissions-list' });
        route.when("/", { redirectTo: '/users/list' });
        route.otherwise({ templateUrl: '/utils-404' });
    }
    ]);
})();
