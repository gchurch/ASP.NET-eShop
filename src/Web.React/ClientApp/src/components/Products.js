import React, { Component } from 'react';
import { Link } from 'react-router-dom';

import './Products.css';

export class Products extends Component {
  static displayName = Products.name;

  constructor(props) {
    super(props);
    this.state = { products: [], loading: true };
  }

  componentDidMount() {
    this.populateProductsData();
  }

  async populateProductsData() {
    const response = await fetch('api/products');
    const data = await response.json();
    this.setState({ products: data, loading: false });
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderProducts(this.state.products);

    return (
      <div>
        <h1 id="tabelLabel" >Products</h1>
        <Link to={"add-product"}>
          <p>Add a product</p>
        </Link>
        {contents}
      </div>
    );
  }

  renderProducts(products) {
    return (
      <div id="products">
        <ul>
          {products.map(product =>
            <li>
              <div>
                <Link to={"product/" + product.productId}>
                  <img src={"images/" + product.imageUrl} alt=""/>
                </Link>
              </div>
              <div>             
                <p id="productTitle"><Link to={"product/" + product.productId}>{ product.title }</Link></p>
                <p id="productSeller">Seller: { product.seller }</p>
                <p id="productPrice">£{ product.price }</p>
                <p id="productDelivery">FREE Delivery</p>
            </div>
            </li>
          )}
        </ul>
      </div>
    );
  }
}
