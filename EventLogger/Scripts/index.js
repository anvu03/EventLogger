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


/**
 * 
 * @param {number} period
 */
function getFailedLogins(period) {
    return $.get({
        url: "/api/weasel/events",
        data: { rollback: period, eventTypeId: 9 },
        cache: false
    });
}

function getPasswordChanges(period) {
    return $.get({
        url: "/api/weasel/events",
        data: { rollback: period, eventTypeId: 11 },
        cache: false
    });
}


function getLogins(period) {
    return $.get({
        url: "/api/weasel/events",
        data: { rollback: period, eventTypeId: 8 },
        cache: false
    });
}