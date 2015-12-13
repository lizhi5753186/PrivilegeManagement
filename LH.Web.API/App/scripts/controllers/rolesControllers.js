(function () {
    var app = angular.module("LH.Controllers");
    app.controller("roles", ["$scope", "$modal", "rolesService", "utils", "language", function ($scope, $modal, rolesService, utils, language) {
        var service = rolesService.list;
        var lang = language(true, "rolesList");
        var methods = {
            lang: lang,
            search: function(isPage) {
                $scope.loadingState = true;
                if (!isPage) $scope.current = 1;
                service.gets({ name: $scope.Name, current: $scope.current, size: $scope.size }).success(function(data) {
                    $scope.list = data.Data;
                    $scope.total = data.Total;
                    $scope.loadingState = false;
                });
            },
            edit: function(item) {
                item.org = angular.copy(item);
                item.isModified = true;
            },
            cancel: function(item) {
                if (item.Id == 0) {
                    utils.remove($scope.list, item);
                    return;
                }
                var org = item.org;
                delete item.org;
                angular.extend(item, org);
                item.isModified = false;
            },
            change: function(item) {
                $modal.open({
                    templateUrl: 'roles-choosePerm',
                    controller: "choosePerm",
                    backdrop: "static",
                    resolve: {
                        params: function() {
                            return item;
                        }
                    }
                });
            },
            append: function() {
                var has = false;
                angular.forEach($scope.list, function(v) {
                    if (v.Id == 0) {
                        has = true;
                    }
                });
                
                if (has) return;
                var obj = { "Permissions": [], "Id": 0, "Name": "", "isModified": true };
                $scope.list.unshift(obj);
            },
            save: function(item) {
                if (!$.trim(item.Name)) {
                    utils.notify(lang.notAllowEmpty, "warning");
                    return;
                }
                if (item.Id == 0) {
                    service.create(item).success(function(data) {
                        if (data.IsCreated) {
                            utils.notify(lang.saveSuccess, "success");
                            item.Id = data.Id;
                            item.isModified = false;
                        }
                    });
                } else {
                    service.update(item).success(function(data) {
                        if (data.IsSaved) {
                            utils.notify(lang.saveSuccess, "success");
                            item.isModified = false;
                        }
                    });
                }
            },
            remove: function(item) {
                var modal = utils.confirm({ msg: lang.confirmDelete, ok: lang.ok, cancel: lang.cancel });
                modal.result.then(function() {
                    service.delete(item.Id).success(function(data) {
                        if (data.IsDeleted) {
                            utils.notify(lang.deleteSuccess, "success");
                            utils.remove($scope.list, item);
                        }
                    });
                });
            },
            size: 10
        };
        
        angular.extend($scope, methods);
        methods.search();
    }]);
    app.controller("choosePerm", ['$scope', "language", "permService", "utils", "$modalInstance", "params", function ($scope, language, permService, utils, $modalInstance, params) {
        var service = permService.list;
        var org = angular.copy(params.Permissions);
        var lang = language(true, "roleChoose");
        var methods = {
            lang: lang,
            ok: function() {
                params.Permissions = org;
                $modalInstance.close(true);
            },
            cancel: function() {
                $modalInstance.dismiss('cancel');
            },
            search: function(isPage) {
                if (!isPage) $scope.current = 1;
                service.gets({ name: $scope.Name, current: $scope.current, size: $scope.size }).success(function(data) {
                    angular.forEach(data.Data, function(l) {
                        angular.forEach(org, function(v) {
                            if (l.Id == v.Id) {
                                l.isChecked = true;
                            }
                        });
                    });
                    
                    $scope.list = data.Data;
                    $scope.total = data.Total;
                });
            },
            checked: function(item) {
                item.isChecked = !item.isChecked;
                if (!item.isChecked) {
                    utils.remove(org, item, function(i, v) {
                        return i.Id == v.Id;
                    });
                } else {
                    org.push(item);
                }
            },
            size: 10
        };
        angular.extend($scope, methods);
        methods.search(false);
    }]);
})()