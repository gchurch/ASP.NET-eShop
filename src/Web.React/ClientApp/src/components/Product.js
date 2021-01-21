import React, {Component} from 'react';

import './Product.css';

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
            <div id="product">
                <div id="image"><img src={"images/" + product.imageUrl} alt=""/></div>
                <p id="productTitle">{product.title}</p>
                <p id="productPrice">Price: <span>£{product.price}</span></p>
                <p id="productQuantity">{product.quantity} <span>in stock</span></p>
                <p id="productDescription"><span>About this product</span><br/>{product.description}</p>
                <p id="productSeller">Seller: <span>{product.seller}</span></p>
            </div>
        );
    }

}