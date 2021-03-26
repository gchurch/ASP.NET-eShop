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
    return (
      <div>
        <h1 id="tabelLabel" >Products</h1>
        <Link to={"add-product"}><p>Add a product</p></Link>
        {this.renderProducts()}
      </div>
    );
  }

  renderProducts() {
    if(this.state.loading) {
      return <p><em>Loading...</em></p>
    }
    else {
      return (
        <div id="products">
          <ul>
            {this.state.products.map(product =>
              <li>
                <div>
                  <Link to={"product/" + product.productId}>
                    <img src={"images/" + product.imageUrl} alt=""/>
                  </Link>
                </div>
                <div>             
                  <p id="productTitle"><Link to={"product/" + product.productId}>{ product.title }</Link></p>
                  <p id="productSeller">Seller: { product.seller }</p>
                  <p id="productPrice"><Link to={"product/" + product.productId}>£{ product.price }</Link></p>
                  <p id="productDelivery">FREE Delivery</p>
              </div>
              </li>
            )}
          </ul>
        </div>
      );
    }
  }
}
