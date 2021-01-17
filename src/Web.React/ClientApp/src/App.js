import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Products } from './components/Products';
import { Counter } from './components/Counter';
import { Product } from './components/Product';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/products' component={Products} />
        <Route path='/product/:id' component={Product} />
      </Layout>
    );
  }
}