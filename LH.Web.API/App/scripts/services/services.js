(function () {
    //创建一个 angularjs 服务模块
    var service = angular.module("LH.Services");
    
    // 为模块创建一个名为linkService服务，linkService返回一个列表对象
    service.factory("linkService", [function () {
        // 定义links为数组类型
        var links = [];
        links.push({
            name: '用户管理', urls: [
                { link: '/users/list', title: '用户管理' },
                { link: '/users/user', title: "添加用户" }
            ]
        });
        
        links.push({
            name: '权限管理', urls: [
                 { link: '/roles/list', title: "角色管理" },
                 { link: '/permissions/list', title: "功能管理" }
            ]
        });
        return links;
    }]);

    service.factory("http", ['$http', '$modal', 'utils', 'language', function($http, $modal, utils, language) {
        var lang = language(true);
        
        var methods = {
            'call': function(type, url, params, data) {
                return $http({ method: type, url: url, params: params, data: data }).success(methods.success).error(methods.errorModal);
            },
            'success': function(data) {
                if (data.Message)
                    utils.confirm({ msg: lang[data.Message], ok: lang.ok });
                return data;
            },
            'errorModal': function(data) {
                $modal.open({
                    templateUrl: 'utils-errorModal ',
                    backdrop: "static",
                    controller: "errorModal",
                    resolve: {
                        error: function() {
                            return data;
                        }
                    }
                });
            },
            'get': function(url, params) {
                return methods.call('GET', url, params);
            },
            'post': function(url, data) {
                return methods.call('POST', url, null, data);
            }
        };
        return methods;
    }]);
    
    service.factory("utils", ["$http", '$modal', function ($http, $modal) {
        var methods = {
            confirm: function(text) {
                return $modal.open({
                    templateUrl: 'utils-confirmModal ',
                    backdrop: "static",
                    controller: "confirmmModal",
                    resolve: {
                        items: function() {
                            return text;
                        }
                    }
                });
            },
            notify: function(content, type) {
                $.notify(content, { type: type, delay: 1000, z_index: 1000000, placement: { from: 'top', align: 'right' } });
            },
            remove: function(list, item, fn) {
                angular.forEach(list, function(i, v) {
                    if (fn ? (fn(i, item)) : (i.$$hashKey === item.$$hashKey)) {
                        list.splice(v, 1);
                        return false;
                    }
                    return true;
                });
            },
            inArray: function(val, array, fn) {
                var has = false;
                angular.forEach(array, function(v) {
                    if (fn && fn(val, v) || val === v) {
                        has = true;
                        return false;
                    }
                    return true;
                });
                return has;
            }
        };
        return methods;
    }]);
    
    service.factory("usersService", ["http", function (http) {
        var methods = {
            list: {
                "get": function (param) {
                    return http.get("/api/user/GetUsers", param);
                },
                "delete": function (id) {
                    return http.post("/api/user/DeleteUser/" + id);
                }
            },
            user: {
                "get": function (param) {
                    return http.get("/api/user/UserInfo", param);
                },
                "update": function (param) {
                    return http.post("/api/user/UpdateUser", param);
                },
                "create": function (param) {
                    return http.post("/api/user/AddUser", param);
                },
                "updateRoles": function (params) {
                    return http.post("/api/user/UpdateRoles", params);
                },
                "deleteRole": function (id, roleId) {
                    return http.post("/api/user/DeleteRole/" + id +"/"+ roleId);
                }
            }
        };
        return methods;
    }]);
    
    service.factory("rolesService", ["http", function (http) {
        var methods = {
            list: {
                "gets": function (param) {
                    return http.get("/api/role/GetRoles", param);
                },
                "create": function (param) {
                    return http.post("/api/role/AddRole", param);
                },
                "update": function (param) {
                    return http.post("/api/role/UpdateRole", param);
                },
                "delete": function (id) {
                    return http.post("/api/role/DeleteRole/" + id);
                }
            }
        };
        return methods;
    }]);

    service.factory("permService", ["http", function (http) {
        var methods = {
            list: {
                "gets": function(params) {
                    return http.get("/api/permission/GetPermissions", params);
                },
                "update": function(params) {
                    return http.put("/api/permission/UpdatePermission", params);
                },
                "create": function(params) {
                    return http.post("/api/permission/AddPermission", params);
                },
                "delete": function(id) {
                    return http.delete("/api/permission/RemovePermission/" + id);
                }
            }
        };
        return methods;
    }]);
    
    service.controller("confirmmModal", ['$scope', '$modalInstance', 'items', function ($scope, $modalInstance, items) {
        var methods = {
            ok: function () {
                $modalInstance.close(true);
            },
            cancel: function () {
                $modalInstance.dismiss('cancel');
            },
            text: items
        };
        $.extend($scope, methods);
    }]);
    
    service.controller("errorModal", ['$scope', '$modalInstance', 'error', function ($scope, $modalInstance, error) {
        var methods = {
            cancel: function() {
                $modalInstance.dismiss('cancel');
            },
            report: function() {
                $modalInstance.close(true);
            }
        };
        angular.extend($scope, methods, error);
    }]);
})()