import React, {Component} from 'react';

import './Product.css';

export class Product extends Component {

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.match.params.id,
            product: {}, 
            loading: true 
        };
    }

    componentDidMount() {
        this.populateProductData();
    }

    async populateProductData() {
        const response = await fetch('api/products/' + this.state.id);
        const data = await response.json();
        this.setState({ product: data, loading: false });
    }

    render () {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderProduct(this.state.product);

        return (
            <div>
                {contents}
            </div>
        );
    }

    renderProduct (product) {
        return (
            <div>
                <div id="product">
                    <div id="image"><img src={"images/" + product.imageUrl} alt=""/></div>
                    <p id="productTitle">{product.title}</p>
                    <p id="productPrice">Price: <span>£{product.price}</span></p>
                    <p id="productQuantity">{product.quantity} <span>in stock</span></p>
                    <p id="productDescription"><span>About this product</span><br/>{product.description}</p>
                    <p id="productSeller">Seller: <span>{product.seller}</span></p>
                </div>
                <button onClick={this.onDelete}>Delete</button>
            </div>
        );
    }

    onDelete = async () => {
        const deletionUrl = "api/products/" + this.state.id
        await fetch(deletionUrl, { method: 'DELETE' });
        console.log("Product deleted");
        this.props.history.push("/products");
    }

}