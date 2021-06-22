class Blog extends React.Component {
    state = { data: this.props.data, url: this.props.url };

    loadCommentsFromServer = () => {
        var xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        }.bind(this);
        xhr.send();
    };

    handleCommentSubmit = comment => {
        var comments = this.state.data.Comments;
        var newComments = [comment];
        if (comments) {
            newComments = comments.concat([comment]);
        }
        this.setState({ data: newComments });

        var data = new FormData();
        data.append('Id', comment.Id);
        data.append('OriginalBlogPostId', this.state.data.Id);
        data.append('Name', comment.Name);
        data.append('EmailAddress', comment.Email);
        data.append('Message', comment.Message);

        var xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = function () {
            this.loadCommentsFromServer();
        }.bind(this);
        xhr.send(data);
    };

    componentDidMount() {
        if (typeof window !== 'undefined') {
            window.setInterval(this.loadCommentsFromServer, this.props.pollInterval);
        }
    }

    render() {
        return (
            <div className="blog">
                <BlogPosts data={this.state.data} />
                <CommentList data={this.state.data} />
                <CommentForm onCommentSubmit={this.handleCommentSubmit} />
            </div>
        );
    }
}

class BlogPosts extends React.Component {
    rawMarkup = () => {
        return { __html: this.props.data.HtmlContent };
    }
    render() {
        return (
            <div className="col-lg-12">
                <h1 className="mt-4">{this.props.data.Title}</h1>
                {/* <hr /> <p>Posted on: { this.calculateDate() }</p> <hr /> */}
                <img className="img-fluid rounded" src={this.props.data.Image} alt={this.props.data.Title} />
                <span dangerouslySetInnerHTML={this.rawMarkup()} />
                <hr />
            </div>
        );
    }
}

class CommentList extends React.Component {
    render() {
        if (this.props.data.Comments) {
            var comments = this.props.data.Comments.map(c => (
                <div key={c.Id} className="comment">
                    <div className="media mb-4">
                        <img className="d-flex mr-3 rounded-circle user-avatar" src={'https://eu.ui-avatars.com/api/?name=' + c.Name} alt={c.Name} />
                        <div className="media-body">
                            <h5 className="mt-0">{c.Name}
                                {/*<small>
                                 * <em>({new Date(r.Date).toDateString()})</em>
                                 * </small>
                                */}</h5>
                            <span>{c.Message}</span>
                        </div>
                    </div>
                    <Replies data={c.Replies} />
                    <ReplyForm data={c} />
                    <hr />
                </div>
            ));
            return <div className="commentList">{comments}</div>;
        }
        else {
            return null;
        }
    }
}

class Replies extends React.Component {
    render() {
        if (this.props.data) {
            var replies = this.props.data.map(r => (
                <div key={r.Id}>
                    <div className="media mb-4">
                        <img className="d-flex mr-3 rounded-circle user-avatar" src={'https://eu.ui-avatars.com/api/?name=' + r.Name} alt={r.Name} />
                        <div className="media-body">
                            <h5 className="mt-0">{r.Name}
                                {/*<small>
                                 * <em>({new Date(r.Date).toDateString()})</em>
                                 * </small>
                                */}                            
                            </h5>
                            <span>{r.Message}</span>
                        </div>
                    </div>
                </div >
            ))
            return <div className="pl-5 pb-3">{replies}</div>;
        }
        else {
            return null;
        }
    }
}


class ReplyForm extends React.Component {
    state = {
        Name: '',
        Message: '',
        Email: ''
    };

    handleNameChange = e => {
        this.setState({ Name: e.target.value });
    };

    handleMessageChange = e => {
        this.setState({ Message: e.target.value });
    };

    handleEmailChange = e => {
        this.setState({ Email: e.target.value });
    };

    handleSubmit = e => {
        e.preventDefault();
        var name = this.state.Name.trim();
        var message = this.state.Message.trim();
        var email = this.state.Email.trim();
        if (!name || !message || !email) {
            return;
        }

        var replies = this.props.data.Replies;
        var newreplies = [this.state];
        if (replies) {
            newreplies = replies.concat([this.state]);
        }
        this.setState({ data: newreplies, Name: '', Message: '', Email: ''  });

        var data = new FormData();
        data.append('id', 0);
        data.append('OriginalBlogPostId', this.props.data.OriginalBlogPostId);
        data.append('OriginalCommentId', this.props.data.Id);
        data.append('Name', name);
        data.append('EmailAddress', email);
        data.append('Message', message);

        var xhr = new XMLHttpRequest();
        xhr.open('post', '/addComment', true);
        xhr.onload = function () {
        }.bind(this);
        xhr.send(data);
    };

    render() {
        return (
            <form className="replyForm pl-4" onSubmit={this.handleSubmit}>
                <div className="card-body pt-0">
                    <div className="form-row">
                        <div className="form-group col-md-6">
                            <input type="text"
                                className="form-control"
                                id="Name"
                                placeholder="Name"
                                value={this.state.Name}
                                onChange={this.handleNameChange}
                                required />
                        </div>
                        <div className="form-group col-md-6">
                            <input type="email"
                                className="form-control"
                                id="Email"
                                placeholder="Email Address"
                                value={this.state.Email}
                                onChange={this.handleEmailChange}
                                required />
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-md-10">
                            <textarea id="Message"
                                className="form-control"
                                rows="1"
                                placeholder="Message"
                                value={this.state.Message}
                                onChange={this.handleMessageChange}
                                required />                            
                        </div>

                        <div className="form-group col-md-2">
                            <input type="submit" className="reply-btn btn btn-primary float-right" value="Reply" />
                        </div>
                    </div>
                </div>
            </form>
        );
    }
}


class CommentForm extends React.Component {
    state = {
        Name: '',
        Message: '',
        Email: ''
    };

    handleNameChange = e => {
        this.setState({ Name: e.target.value });
    };

    handleMessageChange = e => {
        this.setState({ Message: e.target.value });
    };

    handleEmailChange = e => {
        this.setState({ Email: e.target.value });
    };

    handleSubmit = e => {
        e.preventDefault();
        var name = this.state.Name.trim();
        var message = this.state.Message.trim();
        var email = this.state.Email.trim();
        if (!name || !message || !email) {
            return;
        }
        this.props.onCommentSubmit({ Name: name, Message: message, Email: email });
        this.setState({ Name: '', Message: '', Email: '' });
    };

    render() {
        return (
            <form className="commentForm" onSubmit={this.handleSubmit}>
                <div className="card my-4">
                    <h5 className="card-header">Leave a Comment:</h5>
                    <div className="card-body">
                        <div className="form-row">
                            <div className="form-group col-md-6">
                                <label htmlFor="Name">Name</label>
                                <input type="text"
                                    className="form-control"
                                    id="Name"
                                    placeholder="Name"
                                    value={this.state.Name}
                                    onChange={this.handleNameChange}
                                    required
                                />
                            </div>
                            <div className="form-group col-md-6">
                                <label htmlFor="EmailAddress">Email Address</label>
                                <input type="email"
                                    className="form-control"
                                    id="Email"
                                    placeholder="Email Address"
                                    value={this.state.Email}
                                    onChange={this.handleEmailChange}
                                    required
                                />
                            </div>
                        </div>
                        <div className="form-group">
                            <label htmlFor="Message">Message</label>
                            <textarea id="Message"
                                className="form-control"
                                rows="3"
                                value={this.state.Message}
                                onChange={this.handleMessageChange}
                                required
                            />
                        </div>
                        <input type="submit" className="col-md-2 btn btn-primary float-right" value="Post" />
                    </div>
                </div>
            </form>
        );
    }
}