import React, {Component} from 'react';

export class AddProduct extends Component {

    constructor(props) {
        super(props);
        this.state = {
            title: "",
            description: "",
            seller: "",
            price: 0,
            quantity: 0,
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {

        const target = event.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            [name]: value 
        });
    }

    handleSubmit(event) {
        alert('New product created: ' + this.state.title);
        event.preventDefault();
        console.log(this.state);
    }

    render() {
        return (
            <div>
                <h1>Add a Product</h1>
                <form onSubmit={this.handleSubmit}>

                    <div>
                        <p>Product Title: <input name="title" type="text" value={this.state.title} onChange={this.handleChange} /></p>
                    </div>
    
                    <div>
                        <p>Price: £<input name="price" type="number" value={this.state.price} onChange={this.handleChange} /></p>
                    </div>

                    <div>
                        <p>Quantity: <input name="quantity" type="number" value={this.state.quantity} onChange={this.handleChange} /></p>
                    </div>

                    <div>
                        <p>Description: <input name="description" type="text" value={this.state.description} onChange={this.handleChange} /></p>
                    </div>

                    <div>
                        <p>Seller's Name: <input name="seller" type="text" value={this.state.seller} onChange={this.handleChange} /></p>
                    </div>

                    <input type="submit" value="Submit" />

                </form>
            </div>
        );
    }
}