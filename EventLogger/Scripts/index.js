$content = $("#content");


function renderRows(data) {
    var rows;
    /**
{
app_name: "Admin Manager Evaluation By Adjuster",
event_type: "USER_LOGGED_INTO_APP",
count: 1
}
     */
    data.forEach(function(item) {
        rows += `
<tr>
    <td>${item.app_name}</td>
    <td>${item.count}</td>
</tr>
`;
    });

    return rows;
}

/**
 *
 * @param {any} table
 * @param {Array<>} data
 */
function updateTableContent(table, data) {
    var $body = window.$(table).find("tbody");
    $body.empty();
    data.forEach(function(item) {
        $body.append(
            `
<tr>
    <td>${item.app_name}</td>
    <td>${item.count}</td>
</tr>
`
        );
    });
}

function renderPagination() {
    var pagination;
    return pagination;
}

// get logins event 
$.ajax({
    url: "/api/weasel/logins",
    method: "get"
}).done(function(data) {
    $content.append(renderRows(data));
});

// get failed logins 
$.ajax({
    url: "/api/weasel/failed_logins",
    method: "get"
}).done(function(data) {
    $("#failed-login-tbody").append(renderRows(data));
});

// get password changes
$.ajax({
    url: "/api/weasel/password_changes",
    method: "get"
}).done(function(data) {
    $("#password-change-tbody").append(renderRows(data));
});

/**
 * 
 * @param {number} period
 */
function getFailedLogins(period) {
    return $.get({
        url: "/api/weasel/failed_logins",
        data: {},
        cache: false
    });
}

function getPasswordChanges(period) {
    return $.get({
        url: "/api/weasel/password_changes",
        data: {},
        cache: false
    });
}


function getLogins(period) {
    return $.get({
        url: "/api/weasel/logins",
        data: {},
        cache: false
    });
}