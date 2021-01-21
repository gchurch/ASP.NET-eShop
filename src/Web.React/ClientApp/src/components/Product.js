import React, {Component} from 'react';

export class Product extends Component {

    constructor(props) {
        super(props);
        this.state = { product: {}, loading: true };
    }

    componentDidMount() {
        this.populateProductData();
    }

    async populateProductData() {
        const id = this.props.match.params.id;
        const response = await fetch('api/products/' + id);
        const data = await response.json();
        this.setState({ product: data, loading: false });
    }

    render () {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Product.renderProduct(this.state.product);

        return (
            <div>
                {contents}
            </div>
        );
    }

    static renderProduct (product) {
        return (
            <div>
                <h1>Title: {product.title}</h1>
                <p>ID: {product.productId}</p>
            </div>
        );
    }

}