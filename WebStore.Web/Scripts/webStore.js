
//order pager
(function orederPaging() {
    //sorting order
    $("#dropdown").change(function () {
        $("#searchForm").submit();
    })

    //async paging order
    var getOrderPage = function () {
        var a = $(this)

        var options = {
            url: a.attr("href"),
            data: $("#dropdown").serialize(),
            type: "get",
            contentType: "application/json"
        };

        //fixing PagedList bug
        if (a.attr("href") == "" || a.attr("href") == undefined || a.attr("href") == null) {
            return false;
        }

        $.ajax(options).done(function (data) {

            var target = $("#orderListResult")
            $(target).replaceWith(data);
        })
        return false;
    }

    $(".body-content").on("click", ".pagedList #orderPager a", getOrderPage);

}());


//customer paging
(function customerPaging() {
    var getCustomerPage = function () {
        var a = $(this)

        var options = {
            url: a.attr("href"),
            type: "get",
            contentType: "application/json"
        };

        //fixing PagedList bug
        if (a.attr("href") == "" || a.attr("href") == undefined || a.attr("href") == null) {
            return false;
        }

        $.ajax(options).done(function (data) {

            var target = $("#customerListResult")
            $(target).replaceWith(data);

        })
        return false;
    }
    $(".body-content").on("click", ".pagedList #customerPager a", getCustomerPage);
})();


//product paging
(function productPaging() {
    var getProductPage = function () {
        var a = $(this)

        var options = {
            url: a.attr("href"),
            data: $("#languagesSelect").serialize(),
            type: "get",
            contentType: "application/json"
        };

        //fixing PagedList bug
        if (a.attr("href") == "" || a.attr("href") == undefined || a.attr("href") == null) {
            return false;
        }

        $.ajax(options).done(function (data) {

            var target = $("#productListResult")
            $(target).replaceWith(data);
        })
        return false;
    }

    $(".body-content").on("click", ".pagedList #productPager a", getProductPage);

})();

(function getClientPaging() {
    var getClientProductPage = function () {
        var a = $(this)

        var options = {
            url: a.attr("href"),
            type: "get",
            contentType: "application/json"
        };

        //fixing PagedList bug
        if (a.attr("href") == "" || a.attr("href") == undefined || a.attr("href") == null) {
            return false;
        }

        $.ajax(options).done(function (data) {

            var target = $("#productList")
            $(target).replaceWith(data);
        })
        return false;
    }
    $(".body-content").on("click", ".pagedList #clientPager a", getClientProductPage);

})();

//tree
(function treeCategories() {
    var myTree = $('#treeNav');

    //var selectLanguage = $("#languagesSelect").val();

    var dataUrlCategoryTree = "/Client/Store/GetTreeCategoriesJson/";

    console.log(dataUrlCategoryTree)

    myTree.tree({
        dataUrl: dataUrlCategoryTree,
        autoOpen: false,
        dragAndDrop: false,
    });

    myTree.bind(
'tree.select',
 function (event) {
     if (event.node) {
         // node was selected and open
         var node = event.node;
         myTree.tree('openNode', node);
     }
     else {//close node on select if opened
         var node = event.previous_node;
         myTree.tree('closeNode', node);
     }
 })

    var node;

    myTree.bind(
        'tree.click',
    function (event) {
        node = event.node;

        //var myForm = $("#treeForm").serialize();
        var languageIdSelect = $("#languagesSelect").val();
        var pageNumber = $("#pageHidden").val();

        var data1 = {
            id: node.id,
        };


        var wrapper = $("#wrapperContent")
        var productList = $("#productList")

        var attribute = productList.attr("data-productList");

        if (attribute == undefined) {
            var options = {
                url: "/Client/Store/Index",
                data: data1,
                type: "get",
                contentType: "application/json"
            };

            $.ajax(options).done(function (data) {
                var target = wrapper;
                $(target).replaceWith(data);

                //changing the url string from ajax
                var pageurl = $("#hiddenActionUrl").val();
                if (pageurl != window.location.pathname) {
                    window.history.pushState({ path: pageurl }, '', pageurl);
                }
            })
        }
        else {

            var options = {
                url: "/Client/Store/ProductList",
                data: data1,
                type: "get",
                contentType: "application/json"
            };

            $.ajax(options).done(function (data) {
                var target = productList;
                $(target).replaceWith(data);
            })
        }
    });    
})();



//submiting form without ajax
(function changeLanguage() {
    $("#languagesSelect").change(function () {

        $("#hiddenUrl").val(window.location.pathname.toString())

        $("#languageForm").submit();
    });
})();




