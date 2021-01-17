import React, {Component} from 'react';
import { useParams } from 'react-router-dom';

export class Product extends Component {

    constructor(props) {
        super(props);
        this.state = {
            product: null,
            loading: true
        };
    }

    render () {
        return (
            <Body/>
        );
    }
}

function Body() {
    let { id } = useParams();

    return (
        <div>
            <h1>Product</h1>
            <p>ID: {id}</p>
        </div>
    );
}