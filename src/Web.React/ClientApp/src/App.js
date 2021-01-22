import React, { Component } from 'react';
import { Route, Redirect } from 'react-router';
import { Layout } from './components/Layout';
import { Products } from './components/Products';
import { Product } from './components/Product';
import { AddProduct } from './components/AddProduct';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/'>
          <Redirect to='/products' />
        </Route>
        <Route path='/products' component={Products} />
        <Route path='/product/:id' component={Product} />
        <Route path='/add-product' component={AddProduct} />
      </Layout>
    );
  }
}