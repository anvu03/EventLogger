var CommentBox = React.createClass({
    constructor: function () {

    },
    getInitialState: function () {
        return { page: 1 };
    },
    renderPagingControl: function(){
        return;
    },
    render: function () {
        return (
            <div>
                <table className="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>App</th>
                            <th>Count</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td></td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>
                <div>
                    
                </div>
            </div>

        );
    }
});
ReactDOM.render(
    <CommentBox />,
    document.getElementById('content')
);