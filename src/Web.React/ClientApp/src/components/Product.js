import React, {Component} from 'react';

import './Product.css';

export class Product extends Component {

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.match.params.id,
            product: {}, 
            loading: true,
            formValues: {}
        };

        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        this.populateProductData();
    }

    async populateProductData() {
        const response = await fetch('api/products/' + this.state.id);
        const data = await response.json();
        this.setState({ product: data, loading: false, formValues: data });
    }

    render () {
        let productImage = this.renderProductImage(this.state.product);

        let productDetails = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderProductDetails(this.state.product);
        
        let productUpdateForm = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderProductUpdateForm();

        return (
            <div>
                <div id="product">
                    {productImage}
                    {productDetails}
                    {productUpdateForm}
                </div>
                <button onClick={this.onDelete}>Delete</button>
            </div>
        );
    }

    renderProductImage(product) {
        return (
            <div id="image">
                <img src={"images/" + product.imageUrl} alt=""/>
            </div>
        )
    }

    renderProductDetails (product) {
        return (
            <div id="productDetails">
                <p id="productTitle">{product.title}</p>
                <p id="productPrice">Price: <span>£{product.price}</span></p>
                <p id="productQuantity">{product.quantity} <span>in stock</span></p>
                <p id="productDescription"><span>About this product</span><br/>{product.description}</p>
                <p id="productSeller">Seller: <span>{product.seller}</span></p>
            </div>
        );
    }

    onDelete = async () => {
        const deletionUrl = "api/products/" + this.state.id
        await fetch(deletionUrl, { method: 'DELETE' });
        console.log("Product deleted");
        this.props.history.push("/products");
    }

    renderProductUpdateForm () {
        return (
            <form>
                <div>
                    <p>Product Title: <input name="title" type="text" value={this.state.formValues.title} onChange={this.handleChange} /></p>
                </div>
                <div>
                    <p>Price: £<input name="price" type="number" value={this.state.formValues.price} onChange={this.handleChange} /></p>
                </div>
                <div>
                    <p>Quantity: <input name="quantity" type="number" value={this.state.formValues.quantity} onChange={this.handleChange} /></p>
                </div>
                <div>
                    <p>Description: <input name="description" type="text" value={this.state.formValues.description} onChange={this.handleChange} /></p>
                </div>
                <div>
                    <p>Seller's Name: <input name="seller" type="text" value={this.state.formValues.seller} onChange={this.handleChange} /></p>
                </div>
                <input type="submit" value="Submit" />
            </form>
        )
    }

    handleChange(event) {

        const target = event.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            formValues: {
                [name]: value
            }
        });
    }

}