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
      : Products.renderProductsTable(this.state.products);

    return (
      <div>
        <h1 id="tabelLabel" >Products</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  static renderProductsTable(products) {
    return (
      <div id="products">
        <ul>
          {products.map(product =>
            <li>
              <div>
                <img src="assets/{product.imageUrl}" alt=""/>
              </div>
              <div>
                <Link to={"product/" + product.productId}>
                  <p id="productTitle">{ product.title }</p>
                </Link>
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
