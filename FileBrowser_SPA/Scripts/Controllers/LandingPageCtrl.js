var LandingPageCtrl = function ($scope, $window, $location, $http) {
       
    $scope.directory = $window.localStorage['currentPath'] != null ? $window.localStorage['currentPath'] : "drives";
    $scope.hideBack = false;
    $scope.alert = false;

    //Compose URL to make GET requests to server 
    $scope.getUri=function()
    {
        var protocol = $location.protocol();
        var host = $location.host();
        var port = $location.port();

        var baseUrl = protocol + ':\\\\' + host + ':' + port + "\\api\\FileSystemEntries\\";
        return baseUrl;
    }

    //Method that change current path on folder click
    $scope.changePath = function (newValue) {
        $scope.directory = newValue;
    }

    //if the current path was changed (either click on folder or enter directory on the input field), method triggers method that retrieves data from server
    $scope.$watch('directory', function (newVal) {
        $scope.getItems();
        //if ($scope.directory != 'drives') {
        //    $scope.hideBack = false;
        //}
        //else {
        //    $scope.hideBack = true;
        //}
        $window.localStorage['currentPath'] = $scope.directory;

    });

    //method sends GET request to server, that is handled by WebApiController (Get method)
    $scope.getItems = function () {
        $scope.items = [
        {
            name: 'Please wait - loading content, calculating totals...'
        }];
        $scope.hideBack = true;
        $scope.alert = true;
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
            $scope.alert = false;
            $scope.hideBack = false;


        }).error(function (data,status) {
            $scope.alert = true;
            $scope.hideBack = false;
            $scope.count1 = null;
            $scope.count2 = null;
            $scope.count3 = null;
            var message;
            if (status == 404) {
                message = 'The destination path could not be found. Target directory was renamed or deleted.';
            }
            else if (status == 405) {
                message = 'Path is not accessible. Access is denied.';
            }
            else {
                message = 'Internal server error.';

            }
            $scope.items = [{ name: message }];
           
        });
    }
    //encrypt directory name for the URL before sending it to server
    $scope.encryptPath = function (path) {
        newPath = window.encodeURIComponent(path);
        return newPath;
    }

    //get parent directory on click on the ... element of the ul
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
