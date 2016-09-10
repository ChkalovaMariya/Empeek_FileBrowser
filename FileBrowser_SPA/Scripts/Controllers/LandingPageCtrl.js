var LandingPageCtrl = function ($scope, $window, $location, $http) {
       
    $scope.directory = $window.localStorage['currentPath'] != null ? $window.localStorage['currentPath'] : "drives";
    $scope.hideBack = false;

    $scope.getUri=function()
    {
        var protocol = $location.protocol();
        var host = $location.host();
        var port = $location.port();

        var baseUrl = protocol + ':\\\\' + host + ':' + port + "\\api\\FileSystemEntries\\";
        return baseUrl;
    }


    $scope.changePath = function (newValue) {
        $scope.directory = newValue;
    }

    $scope.$watch('directory', function (newVal) {
        $scope.getItems();
        if ($scope.directory != 'drives') {
            $scope.hideBack = false;
        }
        else {
            $scope.hideBack = true;
        }
        $window.localStorage['currentPath'] = $scope.directory;

    });

    $scope.getItems = function () {
        // HTTP GET
        $scope.items = [{ name: 'Please wait - loading content, calculating totals...' }];
        $scope.count1 = null;
        $scope.count2 = null;
        $scope.count3 = null;

        var baseUrl = $scope.getUri();
        var path = $scope.encryptPath($scope.directory);
        $http({
            url: baseUrl + path,
            method: "GET"
        }).success (function (data, status) {
            $scope.items = data.entities;
            $scope.count1 = data.countMin;
            $scope.count2 = data.countMiddle;
            $scope.count3 = data.countMax;
        }).error(function (data,status) {
            
            $scope.count1 = null;
            $scope.count2 = null;
            $scope.count3 = null;
            var message;
            if (status == 400) {
                message = 'The destination path could not be found. Target directory was renamed or deleted.';
            }
            else {
                message = 'Internal server error. Try again later.';
            }
            $scope.items = [{ name: message }];
           
        }        );
    }

    $scope.encryptPath = function (path) {
        //var newPath = path.replace('\\', '!');
        //newPath =  newPath.replace(':', '?');
        newPath = window.encodeURIComponent(path);
        console.log(newPath);
        //newPath = newPath.replace('.', '-');
        return newPath;
    }

    $scope.getParent = function () {
        var end = $scope.directory.lastIndexOf('\\');
        var length = $scope.directory.length - 1;
        var parent;
        if (end != length) {
            parent = ($scope.directory).slice(0, end);
            if (parent.indexOf('\\') == -1) {
                parent = parent + '\\';
            }
        }
        else {
            parent = 'drives';
        }
        $scope.changePath(parent);
    }
            

    $scope.getItems();
}
LandingPageCtrl.$inject = ['$scope', '$window','$location', '$http'];
