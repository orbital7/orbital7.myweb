var _web;
var _panel;
var _webKey;
var _editMode = false;

function loadWeb() {

    $.get({
        url: "/Home/GetWeb?webKey=" + _webKey,
        cache: false,
        success: function (response) {
            _web = response;
            renderWeb();
        },
        error: function (xhr) {
            if (xhr.responseText)
                showPanelMessage(xhr.responseText);
            else
                showPanelMessage("<strong class='ra-color-error'>ERROR:</strong> Web not retrieved");
        }
    });
}

function updateWeb() {

    $.get({
        url: "/Home/GetWeb?webKey=" + _webKey,
        cache: false,
        success: function (response) {
            _web = response;
        }
    });
}

function renderWeb() {

    renderSidebar();

    if (_web.categories.length > 0) {
        selectCategory(_web.categories[0].id);
    }
    else if (!_editMode) {
        showPanelMessage("Use the edit controls to add Categories, Groups, and Sites");
        toggleEditMode();
    }
}

function renderSidebar() {

    var html = "";

    for (var i = 0; i < _web.categories.length; i++) {

        var category = _web.categories[i];
        html += "<table id='" + category.id + "' class='category-item' ";
        html += "onclick='selectCategory(\"" + category.id + "\");'>";
        html += "<tr><td>" + category.name + "</td>";
        html += "<td style='width: 48px'>";
        html += "<div class='btn-group dropright'>";
        html += "<button type='button' class='btn btn-secondary dropdown-toggle edit-element ra-btn-inline' ";
        html += "style='width: 40px;' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>";
        html += "<i class='far fa-edit'></i></button>";
        html += "<div class='dropdown-menu'>";
        html += "<a class='dropdown-item ra-clickable' onclick='beginAddGroup(event, \"" + category.id + "\");'>";
        html += "<button class='btn btn-secondary ra-btn-inline'><i class='fas fa-plus fa ra-btn-add'></i></button> Add Group</a>";
        html += "<a class='dropdown-item ra-clickable' onclick='beginEditCategory(event, \"" + category.id + "\");'>";
        html += "<button class='btn btn-secondary ra-btn-inline'><i class='far fa-edit fa ra-btn-edit'></i></button> Edit Category</a>";
        html += "<a class='dropdown-item ra-clickable' onclick='beginDeleteCategory(event, \"" + category.id + "\");'>";
        html += "<button class='btn btn-secondary ra-btn-inline'><i class='far fa-trash-alt fa ra-btn-delete'></i></button> Delete Category</a>";
        html += "</div>";
        html += "</div></td></tr></table>";
    }

    $(".ra-layout-sidebar").html(html);
}

function selectCategory(categoryId) {

    $(".category-item")
        .removeClass("category-item-selected")
        .addClass("category-item-selectable");

    $("#" + categoryId)
        .removeClass("category-item-selectable")
        .addClass("category-item-selected");

    updateCategory(findCategory(categoryId));
    raHideSidebar();
}

function updateCategory(category) {

    var html = "";

    for (var i = 0; i < category.groups.length; i++) {

        var group = category.groups[i];
        html += getGroupHtml(group);
    }

    _panel.html(html);
    updateEditControls();
}

function findCategory(categoryId) {

    for (var i = 0; i < _web.categories.length; i++) {

        var category = _web.categories[i];
        if (category.id === categoryId)
            return category;
    }

    return null;
}

function findGroup(groupId) {

    for (var i = 0; i < _web.categories.length; i++) {

        var category = _web.categories[i];
        for (var j = 0; j < category.groups.length; j++) {

            var group = category.groups[j];
            if (group.id === groupId)
                return group;
        }
    }

    return null;
}

function findSite(siteId) {

    for (var i = 0; i < _web.categories.length; i++) {

        var category = _web.categories[i];
        for (var j = 0; j < category.groups.length; j++) {

            var group = category.groups[j];
            for (var k = 0; k < group.sites.length; k++) {

                var site = group.sites[k];
                if (site.id === siteId)
                    return site;
            }
        }
    }

    return null;
}

function getGroupHtml(group) {

    var html = "<div id='" + group.id + "'><div class='group-item'>";
    html += "<span class='group-item-name'>" + group.name + "</span>";
    html += "<button class='btn btn-secondary ra-btn-add edit-element ra-btn-inline' onclick='beginAddSite(event, \"" +
        group.id + "\");'><i class='fas fa-plus'></i></button>";
    html += "<button class='btn btn-secondary ra-btn-edit edit-element ra-btn-inline' onclick='beginEditGroup(event, \"" +
        group.id + "\");'><i class='far fa-edit fa'></i></button>";
    html += "<button class='btn btn-secondary ra-btn-delete edit-element ra-btn-inline' onclick='beginDeleteGroup(event, \"" +
        group.id + "\");'><i class='far fa-trash-alt fa'></i></button>";
    html += "</div><div class='group-container'>";

    for (var j = 0; j < group.sites.length; j++) {

        var site = group.sites[j];
        html += getSiteHtml(site);
    }

    html += "</div></div>";

    return html;
}

function updateGroup(group) {

    $("#" + group.id).html(getGroupHtml(group));
    updateEditControls();
}

function getSiteHtml(site) {

    var html = "<span id='" + site.id + "' class='site-item' ";
    html += "onclick='newWindowTo(\"" + site.url + "\");'>";
    html += "<img class='site-thumbnail' src='" + site.thumbnailCachedUrl + "' title='" + site.url + "' />";
    html += "<button class='btn btn-secondary ra-btn-edit edit-element site-edit-button' onclick='beginEditSite(event, \"" +
        site.id + "\");'><i class='far fa-edit fa fa-lg'></i></button>";
    html += "<button class='btn btn-secondary ra-btn-delete edit-element site-delete-button' onclick='beginDeleteSite(event, \"" +
        site.id + "\");'><i class='far fa-trash-alt fa fa-lg'></i></button>";
    html += "</span>";

    return html;
}

function updateSite(site) {

    $("#" + site.id).html(getSiteHtml(site));
    updateEditControls();
}

function showPanelMessage(message) {

    _panel.html(message);
}

function renderSpecifyWebKey(message) {

    _panel.html(message);
}

function updateEditControls() {

    if (_editMode)
        $(".edit-element").visible();
    else
        $(".edit-element").invisible();
}

function toggleEditMode() {

    _editMode = !_editMode;
    updateEditControls();
    $("#editButton").button("toggle");
}

function beginAddCategory(event) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogAddCategory?webKey=" + _webKey,
        "completeAddCategory(response);",
        "Add Category",
        true,
        "Add",
        true,
        "Cancel",
        "medium");
}

function completeAddCategory(web) {

    _web = web;
    renderSidebar();
    selectCategory(_web.categories[_web.categories.length - 1].id);
}

function beginEditCategory(event, categoryId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogUpdateCategory?webKey=" + _webKey + "&categoryId=" + categoryId,
        "completeEditCategory('" + categoryId + "', response);",
        "Edit Category",
        true,
        "Update",
        true,
        "Cancel",
        "medium");
}


function completeEditCategory(categoryId, web) {

    _web = web;
    renderSidebar();
    selectCategory(categoryId);
}

function beginDeleteCategory(event, categoryId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogDeleteCategory?webKey=" + _webKey + "&categoryId=" + categoryId,
        "completeDeleteCategory('" + categoryId + "', response);",
        "Confirm Category Delete",
        true,
        "Yes",
        true,
        "No",
        "small");
}

function completeDeleteCategory(categoryId, web) {

    location.reload();
}

function beginAddGroup(event, categoryId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogAddGroup?webKey=" + _webKey + "&categoryId=" + categoryId,
        "completeAddGroup('" + categoryId + "', response);",
        "Add Group",
        true,
        "Add",
        true,
        "Cancel",
        "medium");
}

function completeAddGroup(categoryId, web) {

    _web = web;
    selectCategory(categoryId);
}

function beginEditGroup(event, groupId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogUpdateGroup?webKey=" + _webKey + "&groupId=" + groupId,
        "completeEditGroup('" + groupId + "', response);",
        "Edit Group",
        true,
        "Update",
        true,
        "Cancel",
        "medium");
}

function completeEditGroup(groupId, web) {

    _web = web;
    var group = findGroup(groupId);
    if (group !== null)
        updateGroup(group);
}

function beginDeleteGroup(event, groupId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogDeleteGroup?webKey=" + _webKey + "&groupId=" + groupId,
        "completeDeleteGroup('" + groupId + "', response);",
        "Confirm Group Delete",
        true,
        "Yes",
        true,
        "No",
        "small");
}

function completeDeleteGroup(groupId, web) {

    _web = web;
    $("#" + groupId).remove();
}

function beginAddSite(event, groupId) {

    event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogAddSite?webKey=" + _webKey + "&groupId=" + groupId,
        "completeAddSite('" + groupId + "', response);",
        "Add Site",
        true,
        "Add",
        true,
        "Cancel",
        "medium");
}

function completeAddSite(groupId, web) {

    _web = web;
    var group = findGroup(groupId);
    if (group !== null)
        updateGroup(group);
}

function beginEditSite(event, siteId) {

    if (event)
        event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogUpdateSite?webKey=" + _webKey + "&siteId=" + siteId,
        "completeEditSite('" + siteId + "', response);",
        "Edit Site",
        true,
        "Update",
        true,
        "Cancel",
        "medium");
}

function completeEditSite(siteId, web) {

    _web = web;
    var site = findSite(siteId);
    if (site !== null)
        updateSite(site);
}

function beginDeleteSite(event, siteId) {

    event.stopPropagation();

    raShowModalDialog(
        "/Home/_DialogDeleteSite?webKey=" + _webKey + "&siteId=" + siteId,
        "completeDeleteSite('" + siteId + "', response);",
        "Confirm Site Delete",
        true,
        "Yes",
        true,
        "No",
        "small");
}

function completeDeleteSite(siteId, web) {

    _web = web;
    $("#" + siteId).remove();
}

function beginSetWeb(event) {

    if (event)
        event.stopPropagation();

    var webKey = "";
    if (_webKey)
        webKey = _webKey;

    raShowModalDialog(
        "/Home/_DialogSetWeb?webKey=" + webKey,
        "completeSetWeb(response);",
        "Set Web Key",
        true,
        "Set",
        true,
        "Cancel",
        "medium");
}

function completeSetWeb(web) {

    _webKey = web.key;
    localStorage.setItem("myweb-webkey", _webKey);

    _web = web;
    renderWeb();
}

$(document).ready(function () {

    _panel = $("#groups-panel");
    _webKey = localStorage.getItem("myweb-webkey");

    if (_webKey) {
        raShowLoading(_panel);
        loadWeb();
    }
    else {
        showPanelMessage("No web has been sepecified for this device");
        beginSetWeb();
    }
});