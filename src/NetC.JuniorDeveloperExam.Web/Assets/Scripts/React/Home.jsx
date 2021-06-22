class Home extends React.Component {
    state = { data: this.props.data };
    render() {
        return (
            <div className="blog">
                <Posts data={this.state.data} />
            </div>
        );
    }
}

class Posts extends React.Component {
    render() {
        const blogPosts = this.props.data.map(post => (
            <div key={post.Id} className="p-4 p-md-5 mb-4 text-white rounded bg-dark">
                <div className="col-md-6 px-0">
                    <h1 className="display-5 fst-italic">{post.Title}</h1>
                    <p className="lead my-3">{ new Date(post.Date).toDateString() }</p>
                    <p className="lead mb-0"><a href={'/blog/' + post.Id} className="text-white fw-bold">Continue reading...</a></p>
                </div>
            </div>


        ));
        return <div className="col-lg-12 p-4">{blogPosts}</div>
    }
}